using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture.TUnit.Extensions;

namespace AutoFixture.TUnit
{
    /// <summary>
    /// An implementation of DataAttribute that composes other DataAttribute instances.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class CompositeDataAttribute : NonTypedDataSourceGeneratorAttribute
    {
        private readonly NonTypedDataSourceGeneratorAttribute[] attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.</param>
        public CompositeDataAttribute(IEnumerable<NonTypedDataSourceGeneratorAttribute> attributes)
            : this(attributes as NonTypedDataSourceGeneratorAttribute[] ?? attributes.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.</param>
        public CompositeDataAttribute(params NonTypedDataSourceGeneratorAttribute[] attributes)
        {
            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        /// Gets the attributes supplied through one of the constructors.
        /// </summary>
        public IReadOnlyList<NonTypedDataSourceGeneratorAttribute> Attributes => Array.AsReadOnly(this.attributes);

        /// <inheritdoc />
        public override IEnumerable<Func<object[]>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
        {
            var testMethod = dataGeneratorMetadata.GetMethod();

            if (testMethod is null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            foreach (var dataSource in this.attributes
                         .Select(attr => attr.GenerateDataSources(dataGeneratorMetadata)))
            {
                foreach (var func in dataSource)
                {
                    yield return func;
                }
            }
        }
    }
}