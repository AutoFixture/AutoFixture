using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.Xunit.v3.Internal
{
    /// <summary>
    ///     Encapsulates access to a property that provides test cases.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    internal class PropertyTestCaseSource : TestCaseSourceBase
    {
        /// <summary>
        ///     Creates an instance of type <see cref="PropertyTestCaseSource" />.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyTestCaseSource(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        /// <summary>
        ///     Gets the source property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <inheritdoc />
        public override IEnumerator GetEnumerator()
        {
            var value = this.PropertyInfo.GetValue(null);
            if (value is not IEnumerable enumerable)
            {
                throw new InvalidCastException("Member does not return an enumerable value.");
            }

            return enumerable.GetEnumerator();
        }
    }
}