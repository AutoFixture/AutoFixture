using System;
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

                // Constructors needs reflection types (e.g. ConstructorInfo, MethodInfo)
                typeof(Ploeh.AutoFixture.Kernel.ConstructorMethod),
                typeof(Ploeh.AutoFixture.Kernel.InstanceMethod),
                typeof(Ploeh.AutoFixture.Kernel.StaticMethod),

                // Autofixture can't create this
                typeof(Ploeh.AutoFixture.Kernel.NoSpecimen),
            };

            var typesToVerify = typeof(IFixture).Assembly
                .GetExportedTypes()
                .Except(typesToExclude)
                .Where(t => !t.IsInterface && !t.IsAbstract);

            equalityBehaviourAssertion.Verify(typesToVerify);
        }
    }
}
