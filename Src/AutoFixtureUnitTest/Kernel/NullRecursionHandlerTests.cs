using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class NullRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Arrange
            // Act
            var sut = new NullRecursionHandler();
            // Assert
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullRecursionHandler();
            // Act
            var dummyRequest = new object();
            var dummyRequests = Enumerable.Empty<object>();
            var actual = sut.HandleRecursiveRequest(dummyRequest, dummyRequests);
            // Assert
            Assert.Null(actual);
        }
    }
}
