using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UriSchemeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new UriSchemeGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new UriSchemeGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var sut = new UriSchemeGenerator();
            var dummyRequest = new object();
            // Act & assert
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Fact]
        public void CreateWithNonUriSchemeRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new UriSchemeGenerator();
            var dummyRequest = new object();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithUriSchemeRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new UriSchemeGenerator();
            var request = typeof(UriScheme);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new UriScheme();
            Assert.Equal(expectedResult, result);
        }
    }
}
