using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a range number to a <see cref="RangedNumberRequest"/>.
    /// </summary>
    public class RangeAttributeRelay : AttributeRelay<RangeAttribute>
    {
        /// <summary>
        /// Creates a <see cref="RangedNumberRequest"/> encapsulating the operand type, the minimum and the maximum of 
        /// the requested number.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", 
            Justification = "All arguments are guaranteed to be non-null by the base class.")]
        protected override object CreateRelayedRequest(ICustomAttributeProvider request, RangeAttribute attribute)
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
                    conversionType = attribute.OperandType;
                }
            }

            Type underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                conversionType = underlyingType;
            }

            return new RangedNumberRequest(
                conversionType,
                Convert.ChangeType(attribute.Minimum, conversionType, CultureInfo.CurrentCulture),
                Convert.ChangeType(attribute.Maximum, conversionType, CultureInfo.CurrentCulture)
                );
        }
    }
}
