using System;
using System.Collections.Generic;
using Ploeh.Albedo;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;
using System.Linq;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class Scenario
    {
        [Fact]
        public void VerifyWritableForProperty()
        {
            var fixture = new Fixture();
            var assertion = new WritablePropertyAssertion(fixture);
            var property = typeof(PropertyHolder<object>).GetProperty("Property");
            assertion.Verify(property);
        }

        [Fact]
        public void VerifyBoundariesForProperty()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var property = typeof(GuardedPropertyHolder<object>).GetProperty("Property");
            assertion.Verify(property);
        }

        [Fact]
        public void VerifyBoundariesForMethod()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var method = typeof(GuardedMethodHost).GetMethod("ConsumeStringAndInt32AndGuid");
            assertion.Verify(method);
        }

        [Fact]
        public void VerifyBoundariesForAllMethods()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var methods = typeof(GuardedMethodHost).GetMethods();
            assertion.Verify(methods);
        }

        [Fact]
        public void VerifyBoundariesForTypeWithRefMethod()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var type = typeof(TypeWithRefMethod<Version>);
            assertion.Verify(type);
        }

        [Fact]
        public void VerifyBoundariesForAllPropertiesOnMutableClass()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var properties = typeof(GuardedPropertyHolder<Version>).GetProperties();
            assertion.Verify(properties);
        }

        [Fact]
        public void VerifyBoundariesForAllPropertiesOnImmutableClass()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var properties = typeof(DoubleParameterType<string, object>).GetProperties();
            assertion.Verify(properties);
        }

        [Fact]
        public void VerifyWritableForAllPropertiesOnMutableClass()
        {
            var fixture = new Fixture();
            var assertion = new WritablePropertyAssertion(fixture);
            var properties = typeof(DoublePropertyHolder<string, int>).GetProperties();
            assertion.Verify(properties);
        }

        [Fact]
        public void VerifyBoundariesForAllConstructors()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var ctors = typeof(GuardedConstructorHost<Version>).GetConstructors();
            assertion.Verify(ctors);
        }

        [Fact]
        public void VerifyBoundariesForAllMembers()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            var members = typeof(GuardedConstructorHost<Version>).GetMembers();
            assertion.Verify(members);
        }

        [Fact]
        public void VerifyReadOnlyPropertyInitializedByConstructor()
        {
            var fixture = new Fixture();
            var assertion = new ConstructorInitializedMemberAssertion(fixture);
            var members = typeof (UnguardedConstructorHost<Version>).GetProperties();
            assertion.Verify(members);
        }

        [Fact]
        public void VerifyConstructorParametersCorrectlyInitializeProperties()
        {
            var fixture = new Fixture();
            var assertion = new ConstructorInitializedMemberAssertion(fixture);
            var members = typeof(MutableValueType).GetConstructors();
            assertion.Verify(members);
        }

        [Fact]
        public void VerifyPublicPropertiesAssignableFromConstructorAreCorrectlyInitialized()
        {
            var fixture = new Fixture();

            var customMatcher = new VisitorEqualityComparer<NameAndType>(
                new NameAndTypeCollectingVisitor(), new NameAndTypeAssignableComparer());

            var assertion = new ConstructorInitializedMemberAssertion(
                fixture, EqualityComparer<object>.Default, customMatcher);

            var properties = typeof(PublicPropertiesAreAssignableFromConstructorParameterTypes)
                .GetProperties();

            assertion.Verify(properties);
        }

        class PublicPropertiesAreAssignableFromConstructorParameterTypes
        {
            public PublicPropertiesAreAssignableFromConstructorParameterTypes(
                string[] bribbets, int[] numbers)
            {
                this.Bribbets = bribbets;
                this.Numbers = numbers;
            }

            public IEnumerable<string> Bribbets { get; private set; }
            public IEnumerable<int> Numbers { get; private set; }

            /// <summary>
            /// Implements the 'copy and update' pattern for immutable objects; the current
            /// object is copied, however a new value for <see cref="Numbers"/> is provided.
            /// </summary>
            /// <returns>A new instance where all values except for <see cref="Numbers"/>
            /// remain the same.</returns>
            public PublicPropertiesAreAssignableFromConstructorParameterTypes WithNumbers(int[] numbers)
            {
                return new PublicPropertiesAreAssignableFromConstructorParameterTypes(
                    (string[])this.Bribbets, numbers);
            }
        }

        class NameAndType
        {
            public NameAndType(string name, Type type)
            {
                this.Name = name;
                this.Type = type;
            }

            public string Name { get; private set; }
            public Type Type { get; private set; }
        }

        class NameAndTypeAssignableComparer : IEqualityComparer<NameAndType>
        {
            public bool Equals(NameAndType x, NameAndType y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
                       && (x.Type.IsAssignableFrom(y.Type) || y.Type.IsAssignableFrom(x.Type));
            }

            public int GetHashCode(NameAndType obj)
            {
                return 0;
            }
        }

        class NameAndTypeCollectingVisitor
            : ReflectionVisitor<IEnumerable<NameAndType>>
        {
            private readonly NameAndType[] values;

            public NameAndTypeCollectingVisitor(
                params NameAndType[] values)
            {
                this.values = values;
            }

            public override IEnumerable<NameAndType> Value
            {
                get { return this.values; }
            }

            public override IReflectionVisitor<IEnumerable<NameAndType>> Visit(
                FieldInfoElement fieldInfoElement)
            {
                if (fieldInfoElement == null) throw new ArgumentNullException("fieldInfoElement");
                var v = new NameAndType(
                    fieldInfoElement.FieldInfo.Name,
                    fieldInfoElement.FieldInfo.FieldType);
                return new NameAndTypeCollectingVisitor(
                    this.values.Concat(new[] { v }).ToArray());
            }

            public override IReflectionVisitor<IEnumerable<NameAndType>> Visit(
                ParameterInfoElement parameterInfoElement)
            {
                if (parameterInfoElement == null) throw new ArgumentNullException("parameterInfoElement");
                var v = new NameAndType(
                    parameterInfoElement.ParameterInfo.Name,
                    parameterInfoElement.ParameterInfo.ParameterType);
                return new NameAndTypeCollectingVisitor(
                    this.values.Concat(new[] { v }).ToArray());
            }

            public override IReflectionVisitor<IEnumerable<NameAndType>> Visit(
                PropertyInfoElement propertyInfoElement)
            {
                if (propertyInfoElement == null) throw new ArgumentNullException("propertyInfoElement");
                var v = new NameAndType(
                    propertyInfoElement.PropertyInfo.Name,
                    propertyInfoElement.PropertyInfo.PropertyType);
                return new NameAndTypeCollectingVisitor(
                    this.values.Concat(new[] { v }).ToArray());
            }
        }

        class VisitorEqualityComparer<T> : IEqualityComparer<IReflectionElement>
        {
            private readonly IReflectionVisitor<IEnumerable<T>> visitor;
            private readonly IEqualityComparer<T> comparer;

            internal VisitorEqualityComparer(
                IReflectionVisitor<IEnumerable<T>> visitor,
                IEqualityComparer<T> comparer)
            {
                this.visitor = visitor;
                this.comparer = comparer;
            }

            bool IEqualityComparer<IReflectionElement>.Equals(IReflectionElement x, IReflectionElement y)
            {
                var values = new CompositeReflectionElement(x, y)
                    .Accept(this.visitor)
                    .Value
                    .ToArray();

                var distinctValues = values.Distinct(this.comparer);
                return values.Length == 2
                       && distinctValues.Count() == 1;
            }

            int IEqualityComparer<IReflectionElement>.GetHashCode(IReflectionElement obj)
            {
                if (obj == null) throw new ArgumentNullException("obj");
                return obj
                    .Accept(this.visitor)
                    .Value
                    .Single()
                    .GetHashCode();
            }
        }

        [Fact]
        public void VerifyCopyAndUpdateWhenThePropertyTypeIsAssignableFromTheParameterType()
        {
            var fixture = new Fixture();

            var customMatcher = new VisitorEqualityComparer<NameAndType>(
                new NameAndTypeCollectingVisitor(), new NameAndTypeAssignableComparer());

            var assertion = new CopyAndUpdateAssertion(
                fixture, EqualityComparer<object>.Default, customMatcher);

            var copyAndUpdateMethod =
                new Methods<PublicPropertiesAreAssignableFromConstructorParameterTypes>()
                    .Select(o => o.WithNumbers(default(int[])));

            assertion.Verify(copyAndUpdateMethod);
        }

        [Fact]
        public void VerifyCompositeEqualityBehaviourOnManyTypes()
        {
            IFixture fixture = new Fixture();
            var equalityBehaviourAssertion = new CompositeIdiomaticAssertion(
                new EqualsNewObjectAssertion(fixture),
                new EqualsNullAssertion(fixture),
                new EqualsSelfAssertion(fixture),
                new EqualsSuccessiveAssertion(fixture));

            var typesToExclude = new[] {
                // Needs parameters of type object to be IComparable
                typeof(Ploeh.AutoFixture.Kernel.RangedNumberRequest),

                // Needs parameters of type IMethod
                typeof(Ploeh.AutoFixture.Kernel.MissingParametersSupplyingMethod),

                // Constructors needs reflection types (e.g. ConstructorInfo, MethodInfo)
                typeof(Ploeh.AutoFixture.Kernel.ConstructorMethod),
                typeof(Ploeh.AutoFixture.Kernel.InstanceMethod),
                typeof(Ploeh.AutoFixture.Kernel.StaticMethod),

                // Autofixture interprets a value of this type as a creation
                // failure
                typeof(Ploeh.AutoFixture.Kernel.NoSpecimen),

                // Takes one or more values involving a generic type argument
                // in the constructor
                typeof(Ploeh.AutoFixture.Kernel.Criterion<>),
                typeof(Ploeh.AutoFixture.Kernel.FieldTypeAndNameCriterion),
                typeof(Ploeh.AutoFixture.Kernel.ParameterTypeAndNameCriterion),
                typeof(Ploeh.AutoFixture.Kernel.PropertyTypeAndNameCriterion),
            };

            var typesToVerify = typeof(IFixture).Assembly
                .GetExportedTypes()
                .Except(typesToExclude)
                .Where(t => !t.IsInterface && !t.IsAbstract);

            equalityBehaviourAssertion.Verify(typesToVerify);
        }
    }
}
