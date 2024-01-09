using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AutoFixture
{
    /// <summary>
    /// Defines methods to create an <see cref="EnumValueCollection{TEnum}"/> that the
    /// <see cref="RestrictedEnumGenerator{TEnum}"/> will use to generate values.
    /// </summary>
    public static class RestrictedEnum
    {
        /// <summary>
        /// Returns a subset of values of <typeparamref name="TEnum"/> that excludes the <paramref name="values"/>
        /// specified.
        /// </summary>
        /// <param name="values">The values of <typeparamref name="TEnum"/> to exclude.</param>
        /// <returns>a <see cref="EnumValueCollection{TEnum}"/> containing a subset of values of
        /// <typeparamref name="TEnum"/> that excludes the <paramref name="values"/> specified.</returns>
        public static EnumValueCollection<TEnum> ExcludeValues<TEnum>(params TEnum[] values)
            where TEnum : struct, Enum
        {
            return ExcludeValues((IEnumerable<TEnum>)values);
        }

        /// <summary>
        /// Returns a subset of values of <typeparamref name="TEnum"/> that excludes the <paramref name="values"/>
        /// specified.
        /// </summary>
        /// <param name="values">The values of <typeparamref name="TEnum"/> to exclude.</param>
        /// <returns>a <see cref="EnumValueCollection{TEnum}"/> containing a subset of values of
        /// <typeparamref name="TEnum"/> that excludes the <paramref name="values"/> specified.</returns>
        public static EnumValueCollection<TEnum> ExcludeValues<TEnum>(IEnumerable<TEnum> values)
            where TEnum : struct, Enum
        {
            return Create(Enumerable.Except, values);
        }

        /// <summary>
        /// Returns a subset of values of <typeparamref name="TEnum"/> that includes only the
        /// <paramref name="values"/> specified.
        /// </summary>
        /// <param name="values">The values of <typeparamref name="TEnum"/> to include.</param>
        /// <returns>a <see cref="EnumValueCollection{TEnum}"/> containing a subset of values of
        /// <typeparamref name="TEnum"/> that includes only the <paramref name="values"/> specified.</returns>
        public static EnumValueCollection<TEnum> IncludeValues<TEnum>(params TEnum[] values)
            where TEnum : struct, Enum
        {
            return IncludeValues((IEnumerable<TEnum>)values);
        }

        /// <summary>
        /// Returns a subset of values of <typeparamref name="TEnum"/> that includes only the
        /// <paramref name="values"/> specified.
        /// </summary>
        /// <param name="values">The values of <typeparamref name="TEnum"/> to include.</param>
        /// <returns>a <see cref="EnumValueCollection{TEnum}"/> containing a subset of values of
        /// <typeparamref name="TEnum"/> that includes only the <paramref name="values"/> specified.</returns>
        public static EnumValueCollection<TEnum> IncludeValues<TEnum>(IEnumerable<TEnum> values)
            where TEnum : struct, Enum
        {
            return Create(Enumerable.Intersect, values);
        }

        private static EnumValueCollection<TEnum> Create<TEnum>(
            Func<IEnumerable<TEnum>, IEnumerable<TEnum>, IEnumerable<TEnum>> setOperation,
            IEnumerable<TEnum> values)
            where TEnum : struct, Enum
        {
            if (values is null)
                throw new ArgumentNullException(nameof(values));

            var enumType = typeof(TEnum);

            var availableValues = Enum.GetValues(enumType).Cast<TEnum>();
            if (!availableValues.Any())
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} is an enum containing no values. Please add at least one value to the enum.",
                    enumType.FullName);

                throw new ArgumentException(message, nameof(values));
            }

            var selectedValues = setOperation(availableValues, values);
            if (!selectedValues.Any())
            {
                const string message = "The values selected results in no available values. " +
                    "Please make sure that at least one value is available.";

                throw new ArgumentException(message, nameof(values));
            }

            return new EnumValueCollection<TEnum>(selectedValues);
        }
    }
}
