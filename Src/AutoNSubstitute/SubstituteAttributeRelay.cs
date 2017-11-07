using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.AutoNSubstitute
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
            if (context == null) throw new ArgumentNullException(nameof(context));

            var customAttributeProvider = request as ICustomAttributeProvider;
            if (customAttributeProvider == null)
            {
                return new NoSpecimen();
            }

            var attribute = customAttributeProvider.GetCustomAttributes(typeof(SubstituteAttribute), true)
                    .OfType<SubstituteAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                return new NoSpecimen();
            }

            object substituteRequest = CreateSubstituteRequest(customAttributeProvider, attribute);
            return context.Resolve(substituteRequest);
        }

        [SuppressMessage("Performance", "CA1801:Review unused parameters",
            Justification = "False positive - request property is used. Bug: https://github.com/dotnet/roslyn-analyzers/issues/1294")]
        private static object CreateSubstituteRequest(ICustomAttributeProvider request, SubstituteAttribute attribute)
        {
            switch (request)
            {
                case ParameterInfo parameter:
                    return new SubstituteRequest(parameter.ParameterType);

                case PropertyInfo property:
                    return new SubstituteRequest(property.PropertyType);

                case FieldInfo field:
                    return new SubstituteRequest(field.FieldType);

                default:
                    throw new NotSupportedException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "{0} is applied to an unsupported code element {1}",
                            attribute,
                            request));
            }
        }
    }
}
