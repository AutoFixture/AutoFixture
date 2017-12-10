using System;
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

            if (!rangedRequest.MemberType.IsEnum())
                return new NoSpecimen();

            var underlyingNumericType = rangedRequest.MemberType.GetTypeInfo().GetEnumUnderlyingType();
            var minimum = rangedRequest.GetConvertedMinimum(underlyingNumericType);
            var maximum = rangedRequest.GetConvertedMaximum(underlyingNumericType);

            var numericValue = context.Resolve(new RangedNumberRequest(underlyingNumericType, minimum, maximum));
            if (numericValue is NoSpecimen)
                return new NoSpecimen();

            return Enum.ToObject(rangedRequest.MemberType, numericValue);
        }
    }
}