using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.Xunit.v3.Internal;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace AutoFixture.Xunit.v3
{
    /// <summary>
    ///     An implementation of DataAttribute that composes other DataAttribute instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class CompositeDataAttribute : DataAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeDataAttribute" /> class.
        /// </summary>
        /// <param name="attributes">
        ///     The attributes representing a data source for a data theory.
        /// </param>
        public CompositeDataAttribute(IEnumerable<DataAttribute> attributes)
            : this(attributes as DataAttribute[] ?? attributes.ToArray())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeDataAttribute" /> class.
        /// </summary>
        /// <param name="attributes">
        ///     The attributes representing a data source for a data theory.
        /// </param>
        public CompositeDataAttribute(params DataAttribute[] attributes)
        {
            this.Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        ///     Gets the attributes supplied through one of the constructors.
        /// </summary>
        public IEnumerable<DataAttribute> Attributes { get; }

        /// <summary>
        ///     Returns the composition of data to be used to test the theory. Favors the data returned
        ///     by DataAttributes in ascending order. Data already returned is ignored on next
        ///     DataAttribute returned data.
        /// </summary>
        /// <param name="testMethod">The method that is being tested.</param>
        /// <returns>
        ///     Returns the composition of the theory data.
        /// </returns>
        /// <remarks>
        ///     The number of combined data sets is restricted to the length of the attribute which provides the fewest data sets.
        /// </remarks>
        public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
        {
            var attributes = this.Attributes;
            var theoryDataRowsEnumerable = attributes.Select(attr => attr.GetData(testMethod, disposalTracker).Result);
            var zip = theoryDataRowsEnumerable.Zip(dataSets => new TheoryDataRow(dataSets.Collapse().ToArray()));

            return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(zip.ToArray());
        }

        public override bool SupportsDiscoveryEnumeration()
        {
            return true;
        }
    }
}