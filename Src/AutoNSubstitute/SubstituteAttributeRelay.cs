using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Relays a request for a code element marked with the <see cref="SubstituteAttribute"/> to a 
    /// <see cref="SubstituteRequest"/> of element's type. 
    /// </summary>
    public class SubstituteAttributeRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a relayed request based on the <see cref="SubstituteAttribute"/> applied to a code element and 
        /// resolves it from the given <paramref name="context"/>.
        /// </summary>
        /// <returns>
        /// A specimen resolved from the <paramref name="context"/> based on a relayed request.
        /// If the <paramref name="request"/> code element does not have <see cref="SubstituteAttribute"/> applied, 
        /// returns <see cref="NoSpecimen"/>.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var customAttributeProvider = request as ICustomAttributeProvider;
            if (customAttributeProvider == null)
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            var attribute = customAttributeProvider.GetCustomAttributes(typeof(SubstituteAttribute), true)
                    .OfType<SubstituteAttribute>().FirstOrDefault();
            if (attribute == null)
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            object substituteRequest = CreateSubstituteRequest(customAttributeProvider, attribute);
            return context.Resolve(substituteRequest);
        }

        private static object CreateSubstituteRequest(ICustomAttributeProvider request, SubstituteAttribute attribute)
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
