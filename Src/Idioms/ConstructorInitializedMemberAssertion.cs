using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a member (property or field) is correctly intialized
    /// by the constructor.
    /// </summary>
    public class ConstructorInitializedMemberAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as the owner of the property, as well as the value to be assigned and read from
        /// the member.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public ConstructorInitializedMemberAssertion(ISpecimenBuilder builder)
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
        /// or properties.
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
                throw new ConstructorInitializedMemberException(constructorInfo, firstParameterNotExposed);
            }
        }

        /// <summary>
        /// Verifies that a property is correctly initialized by the constructor.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <remarks>
        /// <para>
        /// This method verifies that the <paramref name="propertyInfo" /> is correctly initialized with
        /// the value given to the same-named constructor paramter. It uses the <see cref="Builder" /> to
        /// supply values to the constructor(s) of the Type on which the field is implemented, and then
        /// reads from the field. The assertion passes if the value read from the property is the same as
        /// the value passed to the constructor. If more than one constructor has an argument with the
        /// same name and type, all constructors are checked. If any constructor (with a matching argument)
        /// does not initialise the property with the correct value, a 
        /// <see cref="ConstructorInitializedMemberException" /> is thrown.
        /// </para>
        /// </remarks>
        /// <exception cref="WritablePropertyException">The verification fails.</exception>
        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            var expected = this.builder.CreateAnonymous(propertyInfo);
            var matchingConstructors = GetConstructorsWithInitializerForMember(propertyInfo).ToArray();
            
            if (!matchingConstructors.Any())
            {
                if (IsMemberThatRequiresConstructorInitialization(propertyInfo))
                {
                    throw new ConstructorInitializedMemberException(propertyInfo, string.Format(CultureInfo.CurrentCulture,
                        "No constructors with an argument that matches the read-only property '{0}' were found", propertyInfo.Name));
                }

                // For writable properties or fields, having no constructor parameter that initializes
                // the member is perfectly fine.
                return;
            }

            if (matchingConstructors.Select(ci =>
                BuildSpecimenWithMatchingParameterValue(ci, propertyInfo, expected))
                .Select(specimen => propertyInfo.CanRead ? propertyInfo.GetValue(specimen, null) : expected)
                .Any(result => !expected.Equals(result)))
            {
                throw new ConstructorInitializedMemberException(propertyInfo);
            }
        }

        /// <summary>
        /// Verifies that a field is correctly initialized by the constructor.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        /// <remarks>
        /// <para>
        /// This method verifies that <paramref name="fieldInfo" /> is correctly initialized with the
        /// value given to the same-named constructor paramter. It uses the <see cref="Builder" /> to
        /// supply values to the constructor(s) of the Type on which the field is implemented, and then
        /// reads from the field. The assertion passes if the value read from the field is the same as
        /// the value passed to the constructor. If more than one constructor has an argument with the
        /// same name and type, all constructors are checked. If any constructor does not initialise
        /// the field with the correct value, a <see cref="ConstructorInitializedMemberException" /> 
        /// is thrown.
        /// </para>
        /// </remarks>
        /// <exception cref="ConstructorInitializedMemberException">The verification fails.</exception>
        public override void Verify(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            var expected = this.builder.CreateAnonymous(fieldInfo);

            var matchingConstructors = GetConstructorsWithInitializerForMember(fieldInfo).ToArray();
            if (!matchingConstructors.Any())
            {
                if (IsMemberThatRequiresConstructorInitialization(fieldInfo))
                {
                    throw new ConstructorInitializedMemberException(fieldInfo, string.Format(CultureInfo.CurrentCulture,
                        "No constructors with an argument that matches the read-only field '{0}' were found", fieldInfo.Name));
                }

                // For writable properties or fields, having no constructor parameter that initializes
                // the member is perfectly fine.
                return;
            }

            if (matchingConstructors.Select(ci =>
                BuildSpecimenWithMatchingParameterValue(ci, fieldInfo, expected))
                .Select(fieldInfo.GetValue)
                .Any(result => !expected.Equals(result)))
            {
                throw new ConstructorInitializedMemberException(fieldInfo);
            }
        }

        private object BuildSpecimenWithMatchingParameterValue(ConstructorInfo ci, MemberInfo propertyOrField, object expected)
        {
            var paramters = ci.GetParameters();
            var matchingConstructorParameter = paramters.Single(p => IsMatchingParameterAndMember(p, propertyOrField));

            // Create anonymous values for all parameters, except our 'matched' argument
            var paramValues = (from pi in ci.GetParameters()
                let value = pi == matchingConstructorParameter
                    ? expected
                    : this.builder.CreateAnonymous(pi)
                select value).ToArray();

            return ci.Invoke(paramValues.ToArray());
        }

        private static IEnumerable<ConstructorInfo> GetConstructorsWithInitializerForMember(MemberInfo member)
        {
            return member.ReflectedType.GetConstructors().Where(IsConstructorWithMatchingArgument(member));
        }

        private static bool IsMatch(string propertyOrFieldName, Type propertyOrFieldType, string parameterName, Type parameterType)
        {
            return propertyOrFieldName.Equals(parameterName, StringComparison.OrdinalIgnoreCase)
                   && propertyOrFieldType.IsAssignableFrom(parameterType);
        }

        private static bool IsMatchingParameterAndMember(ParameterInfo parameter, MemberInfo fieldOrProperty)
        {
            var fieldInfo = fieldOrProperty as FieldInfo;
            var propertyInfo = fieldOrProperty as PropertyInfo;

            if (fieldInfo == null && propertyInfo == null)
                return false;

            return propertyInfo != null
                ? IsMatchingParameter(propertyInfo)(parameter)
                : IsMatchingParameter(fieldInfo)(parameter);
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

        private static bool IsMemberThatRequiresConstructorInitialization(MemberInfo member)
        {
            var memberAsPropertyInfo = member as PropertyInfo;
            if (memberAsPropertyInfo != null)
            {
                MethodInfo setterMethod = memberAsPropertyInfo.GetSetMethod();
                bool isReadOnly = memberAsPropertyInfo.CanRead &&
                    (setterMethod == null || setterMethod.IsPrivate || setterMethod.IsFamilyOrAssembly || setterMethod.IsFamilyAndAssembly);

                return isReadOnly;
            }

            var memberAsFieldInfo = member as FieldInfo;
            if (memberAsFieldInfo != null)
            {
                bool isReadOnly = memberAsFieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly);
                return isReadOnly;
            }

            return false;
        }

    }
}
