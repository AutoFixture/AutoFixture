using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TrueRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new TrueRequestSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResult()
        {
            // Arrange
            var sut = new TrueRequestSpecification();
            // Act
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Assert
            Assert.True(result);
        }
    }
}
