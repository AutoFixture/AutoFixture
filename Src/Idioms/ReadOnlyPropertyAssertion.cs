using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a read-only property is correctly implemented.
    /// </summary>
    public class ReadOnlyPropertyAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;

        public ReadOnlyPropertyAssertion(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Verifies that all constructor arguments are properly exposed as either fields
        /// or Inspection Properties.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        public override void Verify(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                throw new ArgumentNullException("constructorInfo");

            var parameters = constructorInfo.GetParameters();

            if (parameters.Length == 0)
            {
                // No parameters to verify
                return;
            }

            var propertiesAndFieldsByName = constructorInfo.DeclaringType
	            .GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.MemberType.HasFlag(MemberTypes.Field) || m.MemberType.HasFlag(MemberTypes.Property))
                .ToDictionary(m => m.Name.ToLower(CultureInfo.CurrentCulture));

            var firstMissingParameter = parameters.FirstOrDefault(
                p => !propertiesAndFieldsByName.ContainsKey(p.Name.ToLower(CultureInfo.CurrentCulture)));

            if (firstMissingParameter != null)
            {
                throw new ReadOnlyPropertyException(constructorInfo, firstMissingParameter);
            }
        }

        /// <summary>
        /// Verifies that a read-only property is correctly implemented.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <remarks>
        /// <para>
        /// This method verifies that <paramref name="propertyInfo" /> is correctly implemented as
        /// a read-only property. It used the <see cref="Builder" /> to create an instance of the
        /// Type on which the property is implemented and then reads from the property. The 
        /// assertion passes if the value read from the property is the same as the
        /// value assigned to the constructor. If this is not the case, a
        /// <see cref="ReadOnlyPropertyException" /> is thrown.
        /// </para>
        /// <para>
        /// This method does nothing if the property is not a read-only property.
        /// </para>
        /// </remarks>
        /// <exception cref="WritablePropertyException">The verification fails.</exception>
        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            
            if (propertyInfo.GetGetMethod() == null ||
                propertyInfo.GetSetMethod() != null)
            {
                // Not a read-only property
                return;
            }

            var expected = this.builder.CreateAnonymous(propertyInfo.PropertyType);

            var matchingConstructors = propertyInfo.ReflectedType
                .GetConstructors()
                .Where(IsConstructorWithMatchingArgument(propertyInfo))
                .ToArray();

            if (!matchingConstructors.Any())
            {
                throw new ReadOnlyPropertyException(propertyInfo, string.Format(CultureInfo.CurrentCulture,
                    "No constructors with an argument that matches {0} were found", propertyInfo.Name));
            }

            foreach (var ci in matchingConstructors)
            {
                var paramters = ci.GetParameters();
                var matchingConstructorParameter = paramters.Single(IsMatchingParameter(propertyInfo));

                var paramValues = (from pi in ci.GetParameters()
                                   let value = pi == matchingConstructorParameter
                                        ? expected
                                        : this.builder.CreateAnonymous(pi.ParameterType)
                                   select value).ToList();

                var specimen = ci.Invoke(paramValues.ToArray());
                var result = propertyInfo.GetValue(specimen, null);
                if (!expected.Equals(result))
                {
                    throw new ReadOnlyPropertyException(propertyInfo);
                }                
            }
        }

        private static Func<ParameterInfo, bool> IsMatchingParameter(PropertyInfo propertyInfo)
        {
            return p => p.Name.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase);
        }

        private static Func<ConstructorInfo, bool> IsConstructorWithMatchingArgument(PropertyInfo propertyInfo)
        {
            return c => c.GetParameters().Any(IsMatchingParameter(propertyInfo));
        }
    }
}