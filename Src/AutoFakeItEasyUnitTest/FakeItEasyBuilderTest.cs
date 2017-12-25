using System;
using AutoFixture.Kernel;
using FakeItEasy;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = A.Fake<ISpecimenBuilder>();
            // Act
            var sut = new FakeItEasyBuilder(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyBuilder(null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = A.Fake<ISpecimenBuilder>();
            var sut = new FakeItEasyBuilder(expectedBuilder);
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
        public void CreateWithNonFakeRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var dummyBuilder = A.Fake<ISpecimenBuilder>();
            var sut = new FakeItEasyBuilder(dummyBuilder);
            // Act
            var dummyContext = A.Fake<ISpecimenContext>();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithFakeRequestReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(Fake<object>);
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(new Fake<object>());

            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsAssignableFrom<Fake<object>>(result);
        }

        [Theory]
        [InlineData(typeof(AbstractType), typeof(object))]
        [InlineData(typeof(AbstractType), null)]
        public void CreateWithFakeRequestReturnsCorrectResultWhenContextReturnsNonFake(Type request, object contextValue)
        {
            // Arrange
            var context = new Fake<ISpecimenContext>().FakedObject;
            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context))
                .Returns(contextValue);
            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(Fake<object>), "")]
        [InlineData(typeof(Fake<object>), null)]
        public void CreateFromFakeRequestWhenDecoratedBuilderReturnsNoFakeReturnsCorrectResult(object request, object innerResult)
        {
            // Arrange
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(innerResult);

            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromFakeRequestWhenDecoratedBuilderReturnsFakeOfWrongGenericTypeReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(Fake<IInterface>);
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(new Fake<AbstractType>());

            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}
