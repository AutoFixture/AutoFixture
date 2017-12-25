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
            // Arrange
            // Act
            var sut = new ThrowingRecursionHandler();
            // Assert
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
        }

        [Fact]
        public void HandleRecursiveRequestThrows()
        {
            // Arrange
            var sut = new ThrowingRecursionHandler();
            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(
                () => sut.HandleRecursiveRequest(
                    new object(),
                    new[] { new object() }));
        }
    }
}
