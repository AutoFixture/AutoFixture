using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ThrowingRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Fixture setup
            // Exercise system
            var sut = new ThrowingRecursionHandler();
            // Verify outcome
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
            // Teardown
        }

        [Fact]
        public void HandleRecursiveRequestThrows()
        {
            // Fixture setup
            var sut = new ThrowingRecursionHandler();
            // Exercise system and verify outcome
            Assert.ThrowsAny<ObjectCreationException>(
                () => sut.HandleRecursiveRequest(
                    new object(),
                    new[] { new object() }));
            // Teardown
        }
    }
}
