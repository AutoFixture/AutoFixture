using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Encapsulates a request of a specified type within the specified range.
    /// </summary>
    [PreserveInRequestPath]
    public class RangedRequest : IEquatable<RangedRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangedRequest"/> class.
        /// </summary>
        /// <param name="memberType">Type of the member the range is specified for.</param>
        /// <param name="operandType">Type of the operand.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public RangedRequest(Type memberType, Type operandType, object minimum, object maximum)
        {
            this.MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));
            this.OperandType = operandType ?? throw new ArgumentNullException(nameof(operandType));
            this.Minimum = minimum ?? throw new ArgumentNullException(nameof(minimum));
            this.Maximum = maximum ?? throw new ArgumentNullException(nameof(maximum));
        }

        /// <summary>
        /// Gets the type of the member the range is specified for.
        /// This property defines the expected type of the result.
        /// Refer to the <see cref="OperandType"/> for additional hints about the actual range value type.
        /// </summary>
        public Type MemberType { get; }

        /// <summary>
        /// Gets the specified type of the operand the range is specified for.
        /// This property might not correspond to the actual type of a member for which the Range is specified.
        /// Refer to the <see cref="MemberType"/> property to get the actual member type.
        /// </summary>
        public Type OperandType { get; }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public object Minimum { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public object Maximum { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is RangedRequest other)
            {
                return this.Equals(other);
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(RangedRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.MemberType == other.MemberType &&
                   this.OperandType == other.OperandType &&
                   Equals(this.Minimum, other.Minimum) &&
                   Equals(this.Maximum, other.Maximum);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.MemberType.GetHashCode();
                hashCode = (hashCode * 397) ^ this.OperandType.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Minimum.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Maximum.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Gets minimum value converted to the specified type.
        /// </summary>
        public object GetConvertedMinimum(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            return GetConvertedRangeBoundary(this.Minimum, targetType);
        }

        /// <summary>
        /// Gets maximum value converted to the specified type.
        /// </summary>
        public object GetConvertedMaximum(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            return GetConvertedRangeBoundary(this.Maximum, targetType);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "RangedRequest (MemberType: {0}, OperandType: {1} Minimum: [{2}] {3}, Maximum: [{4}] {5})",
                this.MemberType.FullName,
                this.OperandType.FullName,
                this.Minimum.GetType().Name,
                this.Minimum,
                this.Maximum.GetType().Name,
                this.Maximum);
        }

        private static object GetConvertedRangeBoundary(object attributeValue, Type conversionType)
        {
            if (attributeValue == null) throw new ArgumentNullException(nameof(attributeValue));
            try
            {
                // Mimic RangeAttribute conversion behavior:
                // https://github.com/Microsoft/referencesource/blob/b31308b03e8bd5bf779fb80fda71f31eb959fe0b/System.ComponentModel.DataAnnotations/DataAnnotations/RangeAttribute.cs#L140
                if (attributeValue.GetType() == conversionType)
                    return attributeValue;

                if (conversionType == typeof(int))
                    return Convert.ToInt32(attributeValue, CultureInfo.InvariantCulture);

                if (conversionType == typeof(double))
                    return Convert.ToDouble(attributeValue, CultureInfo.InvariantCulture);

                // Type converter converts much more types, however it doesn't cover some primitive value conversions:
                // https://stackoverflow.com/a/30877647/2009373
                // That's why we resort to Convert.ChangeType() if we detect that we cannot convert.
                var converter = TypeDescriptor.GetConverter(conversionType);
                if (converter.CanConvertFrom(attributeValue.GetType()))
                    return converter.ConvertFrom(attributeValue);

                return Convert.ChangeType(attributeValue, conversionType, CultureInfo.CurrentCulture);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Conversion of RangeAttribute boundary value of {1} type to {2} type caused the overflow " +
                        "exception. Notice, the RangeAttribute type contains the following constructors taking two " +
                        "arguments only:{0}" +
                        "RangeAttribute(int, int){0}" +
                        "RangeAttribute(double, double){0}" +
                        "{0}" +
                        "When you pass a value of other type (e.g. long, decimal), it is converted to either int or " +
                        "double depending on the type. Some conversion (e.g. long to double) could lead to the value " +
                        "distortion due to the double type precision and conversion back to the original type might " +
                        "fail with OverflowException.{0}" +
                        "{0}" +
                        "To solve the issue either specify the range value that could be safely converted to double " +
                        "and back without overflow or use the constructor overload taking value as a string.{0}" +
                        "{0}" +
                        "Example:{0}" +
                        "RangeAttribute(typeof(long), \"0\", \"9223372036854775807\")",
                        Environment.NewLine,
                        attributeValue.GetType().FullName,
                        conversionType.FullName),
                    ex);
            }
        }
    }
}