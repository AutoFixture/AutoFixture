using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.Albedo;
using Ploeh.Albedo.Refraction;
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
        private readonly IEqualityComparer<IReflectionElement> parameterMemberMatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as the owner of the property, as well as the value to be assigned and read from
        /// the member.
        /// </param>
        /// <param name="comparer"> An <see cref="IEqualityComparer"/> instance, which is used 
        /// to determine if each member has the same value which was passed to the matching 
        /// constructor parameter.
        /// </param>
        /// <param name="parameterMemberMatcher">Provides a way to customize the way parameters
        /// are matched to members. The boolean value returned from
        /// <see cref="IEqualityComparer{T}.Equals(T,T)"/> indicates if the parameter and member
        /// are matched.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public ConstructorInitializedMemberAssertion(
            ISpecimenBuilder builder,
            IEqualityComparer comparer,
            IEqualityComparer<IReflectionElement> parameterMemberMatcher)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (parameterMemberMatcher == null)
            {
                throw new ArgumentNullException("parameterMemberMatcher");
            }

            this.builder = builder;
            this.comparer = comparer;
            this.parameterMemberMatcher = parameterMemberMatcher;
        }

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
            : this(
                builder,
                EqualityComparer<object>.Default,
                new DefaultParameterMemberMatcher())
        {
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
        /// This comparer instance is used to determine if all of the value retreived from
        /// the members are equal to their corresponding 'matched' constructor parameter.
        /// </remarks>
        public IEqualityComparer Comparer
        {
            get { return comparer; }
        }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{IReflectionElement}"/> instance which is
        /// used to determine if a constructor parameter matches a given member (property
        /// or field).
        /// </summary>
        /// <remarks>
        /// If the parameter and member are matched, the member is expected to be initialized
        /// from the value passed into the matching constructor parameter.
        /// </remarks>
        public IEqualityComparer<IReflectionElement> ParameterMemberMatcher
        {
            get { return parameterMemberMatcher; }
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

            // Handle backwards-compatibility by replacing the default
            // matcher with one that behaves the similar to the previous
            // behaviour
            IEqualityComparer<IReflectionElement> matcher =
                this.parameterMemberMatcher is DefaultParameterMemberMatcher
                    ? new DefaultParameterMemberMatcher(
                        new DefaultParameterMemberMatcher.NameIgnoreCaseAndTypeAssignableComparer())
                    : this.parameterMemberMatcher;

            var firstParameterNotExposed = parameters.FirstOrDefault(
                p => !publicPropertiesAndFields.Any(m =>
                    matcher.Equals(p.ToReflectionElement(), m.ToReflectionElement())));

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
                    throw new ConstructorInitializedMemberException(propertyInfo, string.Format(CultureInfo.CurrentCulture,
                        "No constructors with an argument that matches the read-only property '{0}' were found", propertyInfo.Name));
                }

                // For writable properties or fields, having no constructor parameter that initializes
                // the member is perfectly fine.
                return;
            }

            var expectedAndActuals = matchingConstructors
                .Select(ctor => BuildSpecimenFromConstructor(ctor, propertyInfo));

            // Compare the value passed into the constructor with the value returned from the property
            if (expectedAndActuals.Any(s => !this.comparer.Equals(s.Expected, s.Actual)))
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
                        "No constructors with an argument that matches the read-only field '{0}' were found", fieldInfo.Name));
                }

                // For writable properties or fields, having no constructor parameter that initializes
                // the member is perfectly fine.
                return;
            }

            var expectedAndActuals = matchingConstructors
                .Select(ctor => BuildSpecimenFromConstructor(ctor, fieldInfo));

            // Compare the value passed into the constructor with the value returned from the property
            if (expectedAndActuals.Any(s => !this.comparer.Equals(s.Expected, s.Actual)))
            {
                throw new ConstructorInitializedMemberException(fieldInfo);
            }
        }

        private ExpectedAndActual BuildSpecimenFromConstructor(
            ConstructorInfo ci, MemberInfo propertyOrField)
        {
            var parametersAndValues = ci.GetParameters()
                .Select(pi => new {Parameter = pi, Value = this.builder.CreateAnonymous(pi)})
                .ToArray();

            // Get the value expected to be assigned to the matching member
            var expectedValueForMember = parametersAndValues
                .Single(p => IsMatchingParameterAndMember(p.Parameter, propertyOrField))
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
                actual = propertyInfo.CanRead
                    ? propertyInfo.GetValue(specimen, null)
                    : expectedValueForMember;
            }
            else
            {
                throw new ArgumentException("Must be a property or field", "propertyOrField");
            }

            return new ExpectedAndActual(expectedValueForMember, actual);
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
            return member.ReflectedType
                .GetConstructors()
                .Where(ci => IsConstructorWithMatchingArgument(ci, member));
        }

        private bool IsMatchingParameterAndMember(ParameterInfo parameter, MemberInfo fieldOrProperty)
        {
            return this.parameterMemberMatcher.Equals(
                fieldOrProperty.ToReflectionElement(), parameter.ToReflectionElement());
        }

        private bool IsConstructorWithMatchingArgument(ConstructorInfo ci, MemberInfo memberInfo)
        {
            return ci.GetParameters().Any(parameterElement =>
                IsMatchingParameterAndMember(parameterElement, memberInfo));
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

        private class DefaultParameterMemberMatcher : ReflectionVisitorElementComparer<NameAndType>
        {
            public class NameIgnoreCaseAndTypeAssignableComparer : IEqualityComparer<NameAndType>
            {
                public bool Equals(NameAndType x, NameAndType y)
                {
                    if (x == null) throw new ArgumentNullException("x");
                    if (y == null) throw new ArgumentNullException("y");
                    return x.Name.Equals(y.Name, StringComparison.CurrentCultureIgnoreCase)
                           && (x.Type.IsAssignableFrom(y.Type) || y.Type.IsAssignableFrom(x.Type));
                }

                public int GetHashCode(NameAndType obj)
                {
                    // Forces methods like Distinct() to use the Equals method, because
                    // the hashcodes will all be equal.
                    return 0;
                }
            }

            public DefaultParameterMemberMatcher(
                IEqualityComparer<NameAndType> comparer)
                : base(new NameAndTypeCollectingVisitor(), comparer)
            {
            }

            public DefaultParameterMemberMatcher()
                : this(null)
            {
            }
        }
    }
}
