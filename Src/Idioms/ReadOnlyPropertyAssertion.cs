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
                return;

            var publicPropertiesAndFields = GetPublicPropertiesAndFields(constructorInfo.DeclaringType).ToArray();
            
            var firstParameterNotExposed = parameters.FirstOrDefault(
                p => !publicPropertiesAndFields.Any(m => IsMatchingParameterAndMember(p, m)));

            if (firstParameterNotExposed != null)
            {
                throw new ReadOnlyPropertyException(constructorInfo, firstParameterNotExposed);
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
            var matchingConstructors = GetConstructorsWithInitializerForMember(propertyInfo).ToList();
            if (!matchingConstructors.Any())
            {
                throw new ReadOnlyPropertyException(propertyInfo, string.Format(CultureInfo.CurrentCulture,
                    "No constructors with an argument that matches {0} were found", propertyInfo.Name));
            }

            if (matchingConstructors.Select(ci =>
                BuildSpecimentWithMatchingParameterValue(ci, propertyInfo, expected))
                .Select(specimen => propertyInfo.GetValue(specimen, null))
                .Any(result => !expected.Equals(result)))
            {
                throw new ReadOnlyPropertyException(propertyInfo);
            }
        }

        /// <summary>
        /// Verifies that a read-only field is correctly implemented.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        /// <remarks>
        /// <para>
        /// This method verifies that <paramref name="fieldInfo" /> is correctly implemented as
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
        public override void Verify(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            var expected = this.builder.CreateAnonymous(fieldInfo.FieldType);

            var matchingConstructors = GetConstructorsWithInitializerForMember(fieldInfo).ToList();
            if (!matchingConstructors.Any())
            {
                throw new ReadOnlyPropertyException(fieldInfo, string.Format(CultureInfo.CurrentCulture,
                    "No constructors with an argument that matches {0} were found", fieldInfo.Name));
            }

            if (matchingConstructors.Select(ci =>
                BuildSpecimentWithMatchingParameterValue(ci, fieldInfo, expected))
                .Select(fieldInfo.GetValue)
                .Any(result => !expected.Equals(result)))
            {
                throw new ReadOnlyPropertyException(fieldInfo);
            }
        }

        private object BuildSpecimentWithMatchingParameterValue(ConstructorInfo ci, MemberInfo propertyOrField, object expected)
        {
            var paramters = ci.GetParameters();
            var matchingConstructorParameter = paramters.Single(p => IsMatchingParameterAndMember(p, propertyOrField));

            // Create anonymous values for all parameters, except our 'matched' argument
            var paramValues = (from pi in ci.GetParameters()
                let value = pi == matchingConstructorParameter
                    ? expected
                    : this.builder.CreateAnonymous(pi.ParameterType)
                select value).ToList();

            return ci.Invoke(paramValues.ToArray());
        }

        private static IEnumerable<ConstructorInfo> GetConstructorsWithInitializerForMember(MemberInfo fieldInfo)
        {
            return fieldInfo.ReflectedType.GetConstructors().Where(IsConstructorWithMatchingArgument(fieldInfo));
        }

        private static bool IsMatch(string propertyOrFieldName, Type propertyOrFieldType, string parameterName, Type parameterType)
        {
            return propertyOrFieldName.Equals(parameterName, StringComparison.OrdinalIgnoreCase)
                   && propertyOrFieldType == parameterType;
        }

        private static bool IsMatchingParameterAndMember(ParameterInfo parameter, MemberInfo fieldOrProperty)
        {
            if (!(fieldOrProperty is FieldInfo) && !(fieldOrProperty is PropertyInfo))
                return false;

            return fieldOrProperty is PropertyInfo
                ? IsMatchingParameter(fieldOrProperty as PropertyInfo)(parameter)
                : IsMatchingParameter(fieldOrProperty as FieldInfo)(parameter);
        }

        private static Func<ParameterInfo, bool> IsMatchingParameter(PropertyInfo propertyInfo)
        {
            return p => IsMatch(propertyInfo.Name, propertyInfo.PropertyType, p.Name, p.ParameterType);
        }

        private static Func<ParameterInfo, bool> IsMatchingParameter(FieldInfo fieldInfo)
        {
            return p => IsMatch(fieldInfo.Name, fieldInfo.FieldType, p.Name, p.ParameterType);
        }

        private static Func<ConstructorInfo, bool> IsConstructorWithMatchingArgument(MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo)
                return c => c.GetParameters().Any(IsMatchingParameter(memberInfo as FieldInfo));

            if (memberInfo is PropertyInfo)
                return c => c.GetParameters().Any(IsMatchingParameter(memberInfo as PropertyInfo));

            throw new ArgumentOutOfRangeException("memberInfo", "must be a property or a field");
        }

        private static IEnumerable<MemberInfo> GetPublicPropertiesAndFields(Type t)
        {
            return t.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.MemberType.HasFlag(MemberTypes.Field) || m.MemberType.HasFlag(MemberTypes.Property));
        }
    }
}