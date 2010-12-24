using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class Scenario
    {
        [Fact]
        public void VerifyBoundaryBehaviorForProperty()
        {
            new Fixture().PickProperty((InvariantReferenceTypePropertyHolder<object> sut) => sut.Property).VerifyBoundaryBehavior();
        }
    }
}
