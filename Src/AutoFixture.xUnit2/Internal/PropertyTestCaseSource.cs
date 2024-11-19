using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    /// <summary>
    /// Encapsulates access to a property that provides test cases.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    public class PropertyTestCaseSource : TestCaseSource
    {
        /// <summary>
        /// Creates an instance of type <see cref="PropertyTestCaseSource"/>.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyTestCaseSource(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        /// <summary>
        /// Gets the source property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <inheritdoc />
        protected override IEnumerable<object[]> GetTestData()
        {
            var value = this.PropertyInfo.GetValue(null);
            if (value is not IEnumerable<object[]> enumerable)
                throw new InvalidCastException("Member does not return an enumerable value.");

            return enumerable;
        }
    }
}