using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a range number to a <see cref="RangedNumberRequest"/>.
    /// </summary>
    public class RangeAttributeRelay : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder relayBuilder = new AttributeRelay<RangeAttribute>(CreateRelayedRequest);

        /// <summary>
        /// Creates a new specimen based on a requested range.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="RangedNumberRequest"/> encapsulating the operand
        /// type, the minimum and the maximum of the requested number, if possible; otherwise,
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            return this.relayBuilder.Create(request, context);
        }

        private static object CreateRelayedRequest(ICustomAttributeProvider request, RangeAttribute rangeAttribute)
        {
            Type conversionType = null;

            var pi = request as PropertyInfo;
            if (pi != null)
            {
                conversionType = pi.PropertyType;
            }
            else
            {
                var fi = request as FieldInfo;
                if (fi != null)
                {
                    conversionType = fi.FieldType;
                }
                else
                {
                    conversionType = rangeAttribute.OperandType;
                }
            }

            Type underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                conversionType = underlyingType;
            }

            return new RangedNumberRequest(
                conversionType,
                Convert.ChangeType(rangeAttribute.Minimum, conversionType, CultureInfo.CurrentCulture),
                Convert.ChangeType(rangeAttribute.Maximum, conversionType, CultureInfo.CurrentCulture)
                );
        }
    }
}
