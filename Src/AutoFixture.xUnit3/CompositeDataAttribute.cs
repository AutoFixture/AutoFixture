using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.Xunit3.Internal;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace AutoFixture.Xunit3
{
    /// <summary>
    /// An implementation of DataAttribute that composes other DataAttribute instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class CompositeDataAttribute : DataAttribute
    {
        private readonly DataAttribute[] attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.</param>
        public CompositeDataAttribute(IEnumerable<DataAttribute> attributes)
            : this(attributes as DataAttribute[] ?? attributes.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.</param>
        public CompositeDataAttribute(params DataAttribute[] attributes)
        {
            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        /// Gets the attributes supplied through one of the constructors.
        /// </summary>
        public IReadOnlyList<DataAttribute> Attributes => Array.AsReadOnly(this.attributes);

        /// <inheritdoc />
        public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
            MethodInfo testMethod, DisposalTracker disposalTracker)
        {
            if (testMethod is null) throw new ArgumentNullException(nameof(testMethod));

            var dataRowSources = this.attributes
                .Select(attr => attr.GetData(testMethod, disposalTracker).AsTask())
                .ToArray();
            var results = await Task.WhenAll(dataRowSources);
            var theoryRows = results
                .Select(x => x.Select(a => a.GetData()))
                .Zip(dataSets => new TheoryDataRow(dataSets.Collapse().ToArray()))
                .ToArray();
            return theoryRows;
        }

        /// <inheritdoc />
        public override bool SupportsDiscoveryEnumeration() => false;
    }
}