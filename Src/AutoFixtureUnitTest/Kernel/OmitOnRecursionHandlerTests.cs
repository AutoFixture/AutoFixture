using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class OmitOnRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Arrange
            // Act
            var sut = new OmitOnRecursionHandler();
            // Assert
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new OmitOnRecursionHandler();
            // Act
            var dummyRequest = new object();
            var dummyRequests = Enumerable.Empty<object>();
            var actual = sut.HandleRecursiveRequest(dummyRequest, dummyRequests);
            // Assert
            var expected = new OmitSpecimen();
            Assert.Equal(expected, actual);
        }
    }
}
