using System;
using AutoFixture.Kernel;
using Rhino.Mocks;
using Xunit;

namespace AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockAroundAdviceTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = MockRepository.GenerateMock<ISpecimenBuilder>();
            // Act
            var sut = new RhinoMockAroundAdvice(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new RhinoMockAroundAdvice((ISpecimenBuilder)null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = MockRepository.GenerateMock<ISpecimenBuilder>();
            var sut = new RhinoMockAroundAdvice(expectedBuilder);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateWithNonMockRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var dummyBuilder = MockRepository.GenerateMock<ISpecimenBuilder>();
            var sut = new RhinoMockAroundAdvice(dummyBuilder);
            // Act
            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}
