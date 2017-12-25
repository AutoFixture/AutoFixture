using System;
using AutoFixture.Kernel;
using Moq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class MockPostprocessorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = new Mock<ISpecimenBuilder>().Object;
            // Act
            var sut = new MockPostprocessor(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MockPostprocessor(null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new Mock<ISpecimenBuilder>().Object;
            var sut = new MockPostprocessor(expectedBuilder);
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
        [InlineData(typeof(Mock))]
        [InlineData(typeof(Mock<>))]
        public void CreateWithNonMockRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var dummyBuilder = new Mock<ISpecimenBuilder>().Object;
            var sut = new MockPostprocessor(dummyBuilder);
            // Act
            var dummyContext = new Mock<ISpecimenContext>().Object;
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateMockRequestReturnsCorrectMock()
        {
            // Arrange
            var request = typeof(Mock<object>);
            var context = new Mock<ISpecimenContext>().Object;

            var builderStub = new Mock<ISpecimenBuilder>();
            builderStub.Setup(b => b.Create(request, context)).Returns(new Mock<object>());

            var sut = new MockPostprocessor(builderStub.Object);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var mock = Assert.IsAssignableFrom<Mock<object>>(result);
            Assert.True(mock.CallBase);
            Assert.Equal(DefaultValue.Mock, mock.DefaultValue);
        }

        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void CreateFromMockRequestWhenDecoratedBuilderReturnsValidNonMockSpecimenReturnsCorrectResult(
            object validNonMockSpecimen)
        {
            // Arrange
            var request = typeof (Mock<object>);
            var context = new Mock<ISpecimenContext>().Object;

            var builderStub = new Mock<ISpecimenBuilder>();
            builderStub.Setup(b => b.Create(request, context)).Returns(validNonMockSpecimen);

            var sut = new MockPostprocessor(builderStub.Object);
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(validNonMockSpecimen, result);
        }

        [Fact]
        public void CreateFromMockRequestWhenDecoratedBuilderReturnsMockOfWrongGenericTypeReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(Mock<IInterface>);
            var context = new Mock<ISpecimenContext>().Object;

            var builderStub = new Mock<ISpecimenBuilder>();
            builderStub.Setup(b => b.Create(request, context)).Returns(new Mock<AbstractType>());

            var sut = new MockPostprocessor(builderStub.Object);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}
