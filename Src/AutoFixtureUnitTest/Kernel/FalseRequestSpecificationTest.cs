using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FalseRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new FalseRequestSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FalseRequestSpecification();
            // Exercise system
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}
