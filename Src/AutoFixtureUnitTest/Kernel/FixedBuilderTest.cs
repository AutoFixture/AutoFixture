using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class FixedBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummySpecimen = new object();
            // Act
            var sut = new FixedBuilder(dummySpecimen);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var expectedSpecimen = new object();
            var sut = new FixedBuilder(expectedSpecimen);
            // Act
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
