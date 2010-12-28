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
    }
}
