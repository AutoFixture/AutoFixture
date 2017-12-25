using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomBooleanSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RandomBooleanSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new RandomBooleanSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Arrange
            var sut = new RandomBooleanSequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonBooleanRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonBooleanRequest = new object();
            var sut = new RandomBooleanSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonBooleanRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResult()
        {
            // Arrange
            var booleanRequest = typeof(bool);
            var sut = new RandomBooleanSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(booleanRequest, dummyContainer);
            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
