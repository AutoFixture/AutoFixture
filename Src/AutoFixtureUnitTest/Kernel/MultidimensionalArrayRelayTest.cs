using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultidimensionalArrayRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new MultidimensionalArrayRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        } 
    }
}