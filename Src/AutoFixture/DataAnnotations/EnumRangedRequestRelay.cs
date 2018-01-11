using System;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Handles <see cref="RangedRequest"/> of enum type by forwarding requests
    /// to the <see cref="RangedNumberRequest"/> of the underlying enum type.
    /// </summary>
    public class EnumRangedRequestRelay : ISpecimenBuilder
    {
        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rangedRequest = request as RangedRequest;
            if (rangedRequest == null)
                return new NoSpecimen();

            if (!rangedRequest.MemberType.GetTypeInfo().IsEnum)
                return new NoSpecimen();

            var underlyingNumericType = rangedRequest.MemberType.GetTypeInfo().GetEnumUnderlyingType();

            object numericMin;
            object numericMax;
            if (rangedRequest.Minimum.GetType().IsNumberType())
            {
                numericMin = rangedRequest.GetConvertedMinimum(underlyingNumericType);
                numericMax = rangedRequest.GetConvertedMaximum(underlyingNumericType);
            }
            else
            {
                var enumMin = rangedRequest.GetConvertedMinimum(rangedRequest.MemberType);
                var enumMax = rangedRequest.GetConvertedMaximum(rangedRequest.MemberType);

                numericMin = Convert.ChangeType(enumMin, underlyingNumericType, CultureInfo.CurrentCulture);
                numericMax = Convert.ChangeType(enumMax, underlyingNumericType, CultureInfo.CurrentCulture);
            }

            var numericValue = context.Resolve(new RangedNumberRequest(underlyingNumericType, numericMin, numericMax));
            if (numericValue is NoSpecimen)
                return new NoSpecimen();

            return Enum.ToObject(rangedRequest.MemberType, numericValue);
        }
    }
}