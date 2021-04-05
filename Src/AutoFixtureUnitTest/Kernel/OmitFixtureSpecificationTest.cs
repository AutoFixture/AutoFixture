using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class OmitFixtureSpecificationTest
    {
        [Fact]
        public void SutIsSpecification()
        {
            // Arrange
            // Act
            var sut = new OmitFixtureSpecification();

            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Theory]
        [InlineData(typeof(Fixture))]
        [InlineData(typeof(IFixture))]
        public void IsSatisfiedByReturnsCorrectResultForFixtureRequests(object request)
        {
            // Arrange
            var sut = new OmitFixtureSpecification();

            // Act
            var result = sut.IsSatisfiedBy(request);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(typeof(string))]
        [InlineData(0)]
        [InlineData(typeof(int))]
        public void IsSatisfiedByReturnsCorrectResultForNonFixtureRequests(object request)
        {
            // Arrange
            var sut = new OmitFixtureSpecification();

            // Act
            var result = sut.IsSatisfiedBy(request);

            // Assert
            Assert.True(result);
        }
    }
}