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
            new Fixture().ForProperty((PropertyHolder<object> sut) => sut.Property).VerifyWritable();
        }

        [Fact]
        public void VerifyBoundariesForProperty()
        {
            new Fixture().ForProperty((InvariantReferenceTypePropertyHolder<object> sut) => sut.Property).VerifyBoundaries();
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
    }
}
