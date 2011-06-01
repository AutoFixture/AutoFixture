using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

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
            new Fixture().ForProperty((GuardedPropertyHolder<object> sut) => sut.Property).VerifyBoundaries();
        }

        [Fact]
        public void VerifyBoundariesForMethod()
        {
            new Fixture().ForMethod((GuardedMethodHost sut, string s, int i, Guid g) => sut.ConsumeStringAndInt32AndGuid(s, i, g)).VerifyBoundaries();
        }

        [Fact]
        public void VerifyBoundariesForAllMethods()
        {
            new Fixture().ForAllMethodsOf<GuardedMethodHost>().VerifyBoundaries();
        }

        [Fact]
        public void VerifyBoundariesForAllPropertiesOnMutableClass()
        {
            new Fixture().ForAllPropertiesOf<GuardedPropertyHolder<Version>>().VerifyBoundaries();
        }

        [Fact]
        public void VerifyBoundariesForAllPropertiesOnImmutableClass()
        {
            new Fixture().ForAllPropertiesOf<DoubleParameterType<string, object>>().VerifyBoundaries();
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
            new Fixture().ForAllConstructorsOf<GuardedConstructorHost<Version>>().VerifyBoundaries();
        }

        [Fact]
        public void VerifyBoundariesForAllMembers()
        {
            new Fixture().ForAllMembersOf<GuardedConstructorHost<Version>>().VerifyBoundaries();
        }
    }
}
