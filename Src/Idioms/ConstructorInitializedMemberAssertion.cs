using System;
using System.Collections;
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
        private readonly IEqualityComparer comparer;
        private readonly IParameterMemberMatcher matcher;

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
            : this(builder, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberAssertion"/>
        /// class.
        /// </summary>
        /// <param name="builder">
        ///     A composer which can create instances required to implement the idiomatic unit test,
        ///     such as the owner of the property, as well as the value to be assigned and read from
        ///     the member.
        /// </param>
        /// <param name="matcher">An <see cref="IParameterMemberMatcher"/> which can match constructor
        /// parameters to members, returning true for a match, false otherwise.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer"/> instance used to determine if 
        /// the member has the same value that was passed to the matching constructor parameter.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public ConstructorInitializedMemberAssertion(
            ISpecimenBuilder builder,
            IParameterMemberMatcher matcher,
            IEqualityComparer comparer)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
            this.comparer = comparer ?? new DefaultEqualityComparer();
            this.matcher = matcher ?? new DefaultMatcher();
        }

        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the comparer supplied to the constructor.
        /// </summary>
        /// <remarks>
        /// This comparer instance is used to determine if the value retreived from
        /// the member is equal to the value passed to the constructor instance.
        /// </remarks>
        public IEqualityComparer Comparer
        {
            get { return this.comparer; }
        }

        /// <summary>
        /// Gets the <see cref="IParameterMemberMatcher"/> which is used to determine if
        /// a constructor parameter matches a given member (property or field).
        /// </summary>
        /// <remarks>
        /// If the parameter and member 'match', then the value given to the 
        /// constructor and the value read from the member are compared using the
        /// <see cref="Comparer"/>.
        /// </remarks>
        public IParameterMemberMatcher Matcher
        {
            get { return this.matcher; }
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
                p => !publicPropertiesAndFields.Any(m => this.matcher.IsMatch(p, m)));

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

            var matchingConstructors = GetConstructorsWithInitializerForMember(propertyInfo).ToArray();

            if (!matchingConstructors.Any())
            {
                if (IsMemberThatRequiresConstructorInitialization(propertyInfo))
                {
                    throw new ConstructorInitializedMemberException(propertyInfo,
                        string.Format(CultureInfo.CurrentCulture,
                            "No constructors with an argument that matches the read-only property '{0}' were found",
                            propertyInfo.Name));
                }

                // For writable properties or fields, having no constructor parameter that initializes
                // the member is perfectly fine.
                return;
            }

            var valuesFromSpecimens = matchingConstructors
                .Select(ci => BuildValuesFromSpecimens(ci, propertyInfo));

            // Compare the value passed into the constructor with the value returned from the property
            if (valuesFromSpecimens.Any(s => !this.comparer.Equals(s.Expected, s.Actual)))
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

            var matchingConstructors = GetConstructorsWithInitializerForMember(fieldInfo).ToArray();
            if (!matchingConstructors.Any())
            {
                if (IsMemberThatRequiresConstructorInitialization(fieldInfo))
                {
                    throw new ConstructorInitializedMemberException(fieldInfo, string.Format(CultureInfo.CurrentCulture,
                        "No constructors with an argument that matches the read-only field '{0}' were found",
                        fieldInfo.Name));
                }

                // For writable properties or fields, having no constructor parameter that initializes
                // the member is perfectly fine.
                return;
            }

            var valuesFromSpecimens = matchingConstructors
                .Select(ctor => BuildValuesFromSpecimens(ctor, fieldInfo));

            // Compare the value passed into the constructor with the value returned from the property
            if (valuesFromSpecimens.Any(s => !this.comparer.Equals(s.Expected, s.Actual)))
            {
                throw new ConstructorInitializedMemberException(fieldInfo);
            }
        }

        private ExpectedAndActual BuildValuesFromSpecimens(
            ConstructorInfo ci, MemberInfo propertyOrField)
        {
            // Create anonymous values for all parameters, except our 'matched' argument
            var parametersAndValues = ci.GetParameters()
                .Select(pi => new {Parameter = pi, Value = this.builder.CreateAnonymous(pi)})
                .ToArray();

            // Get the value expected to be assigned to the matching member
            var expectedValueForMember = parametersAndValues
                .Single(p => this.matcher.IsMatch(p.Parameter, propertyOrField))
                .Value;

            // Construct an instance of the specimen class
            var specimen = ci.Invoke(parametersAndValues.Select(pv => pv.Value).ToArray());

            // Get the value from the specimen field/property
            object actual;
            if (propertyOrField is FieldInfo)
            {
                actual = (propertyOrField as FieldInfo).GetValue(specimen);
            }
            else if (propertyOrField is PropertyInfo)
            {
                var propertyInfo = propertyOrField as PropertyInfo;
                actual = propertyInfo.CanRead ? propertyInfo.GetValue(specimen, null) : expectedValueForMember;
            }
            else
            {
                throw new ArgumentException("Must be a property or field", "propertyOrField");
            }

            return new ExpectedAndActual(
                expected: expectedValueForMember,
                actual: actual);
        }

        private class ExpectedAndActual
        {
            public ExpectedAndActual(object expected, object actual)
            {
                this.Expected = expected;
                this.Actual = actual;
            }

            public object Expected { get; private set; }
            public object Actual { get; private set; }
        }

        private IEnumerable<ConstructorInfo> GetConstructorsWithInitializerForMember(MemberInfo member)
        {
            return member.ReflectedType.GetConstructors().Where(IsConstructorWithMatchingArgument(member));
        }

        /// <summary>
        /// Returns a function that, when executed, returns true if the constructor has any argument
        /// that matches the provided member.
        /// </summary>
        private Func<ConstructorInfo, bool> IsConstructorWithMatchingArgument(MemberInfo memberInfo)
        {
            return c => c.GetParameters().Any(p => this.matcher.IsMatch(p, memberInfo));
        }

        private sealed class DefaultMatcher : IParameterMemberMatcher
        {
            public bool IsMatch(ParameterInfo parameter, MemberInfo member)
            {
                return IsMatchingParameterAndMember(parameter, member);
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

            private static bool IsMatch(string propertyOrFieldName, Type propertyOrFieldType, string parameterName,
                Type parameterType)
            {
                return propertyOrFieldName.Equals(parameterName, StringComparison.OrdinalIgnoreCase)
                       && propertyOrFieldType.IsAssignableFrom(parameterType);
            }

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

        private sealed class DefaultEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object expected, object actual)
            {
                if (expected == null && actual == null)
                    return true;

                if (expected != null && expected.Equals(actual))
                    return true; // the objects are equal

                // Handle the scenario where the constructor argument was a more specific type
                // than what is exposed by the public member field/property
                // For example: .ctor(string[]) exposed as IEnumerable<string> 
                // OR .ctor(WoodenChair) exposed as IChair
                if (actual != null
                    && actual.GetType().IsInstanceOfType(expected))
                {
                    return actual.Equals(expected);
                }

                return false;
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return 0;
            }
        }

    }
}