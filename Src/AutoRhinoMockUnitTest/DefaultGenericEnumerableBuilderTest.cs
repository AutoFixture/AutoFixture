using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace AutoRhinoMockUnitTest
{
    public class DefaultGenericEnumerableBuilderTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.CreateAnonymous<DefaultGenericEnumerableBuilder>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(result);
            // Teardown
        }


    }
}
