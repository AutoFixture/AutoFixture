using System;
using System.Globalization;
using System.Reflection;
using Ploeh.AutoFixture.DataAnnotations;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Relays a request for a code element marked with the <see cref="SubstituteAttribute"/> to a 
    /// <see cref="SubstituteRequest"/> of element's type. 
    /// </summary>
    public class SubstituteAttributeRelay : AttributeRelay<SubstituteAttribute>
    {
        /// <summary>
        /// Creates a <see cref="SubstituteRequest"/> with type of the code element represented by the 
        /// <paramref name="request"/>.
        /// </summary>
        /// <param name="request">
        /// A <see cref="ParameterInfo"/>, <see cref="PropertyInfo"/> or <see cref="FieldInfo"/> instance representing
        /// the code element to which the <paramref name="attribute"/> is applied.
        /// </param>
        /// <param name="attribute">
        /// A <see cref="SubstituteAttribute"/> applied to the code element described by the <paramref name="request"/>.
        /// </param>
        protected override object CreateRelayedRequest(ICustomAttributeProvider request, SubstituteAttribute attribute)
        {
            var parameter = request as ParameterInfo;
            if (parameter != null)
            {
                return new SubstituteRequest(parameter.ParameterType);
            }

            var property = request as PropertyInfo;
            if (property != null)
            {
                return new SubstituteRequest(property.PropertyType);
            }

            var field = request as FieldInfo;
            if (field != null)
            {
                return new SubstituteRequest(field.FieldType);
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} is applied to an unsupported code element {1}",
                    attribute, request));
        }
    }
}
