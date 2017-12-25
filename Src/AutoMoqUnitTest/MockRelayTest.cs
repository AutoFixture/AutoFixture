using System;
using System.Reflection;
using AutoFixture.Kernel;
using Moq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class MockRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new MockRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MockRelay(null));
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Arrange
            var expected = new Mock<IRequestSpecification>().Object;
            var sut = new MockRelay(expected);
            // Act
            IRequestSpecification result = sut.MockableSpecification;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new MockRelay();
            // Act
            var result = sut.MockableSpecification;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new MockRelay();
            var dummyRequest = new object();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateWithNonAbstractionRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new MockRelay();
            var dummyContext = new Mock<ISpecimenContext>().Object;
            // Act
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequestReturnsCorrectResult(Type request)
        {
            // Arrange
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var mock = (Mock)Activator.CreateInstance(mockType);
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(mock);

            var sut = new MockRelay();
            // Act
            var result = sut.Create(request, contextStub.Object);
            // Assert
            Assert.Equal(mock.Object, result);
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonMock()
        {
            // Arrange
            var request = typeof(IInterface);
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(new object());

            var sut = new MockRelay();
            // Act
            var result = sut.Create(request, contextStub.Object);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void CreateReturnsCorrectResultWhenContextReturnsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Arrange
            var request = typeof (IInterface);
            var mockType = typeof (Mock<>).MakeGenericType(request);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(validNonMockSpecimen);

            var sut = new MockRelay();
            // Act
            var result = sut.Create(request, contextStub.Object);
            // Assert
            Assert.Equal(validNonMockSpecimen, result);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateReturnsCorrectResultWhenSpecificationIsSatisfied(
            Type request)
        {
            // Arrange
            var specificationStub = new Mock<IRequestSpecification>();
            specificationStub
                .Setup(s => s.IsSatisfiedBy(request))
                .Returns(true);

            var expected = (Mock)typeof(Mock<>)
                .MakeGenericType(request)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);
            var contextStub = new Mock<ISpecimenContext>();
            contextStub
                .Setup(c => c.Resolve(typeof(Mock<>).MakeGenericType(request)))
                .Returns(expected);

            var sut = new MockRelay(specificationStub.Object);
            // Act
            var actual = sut.Create(request, contextStub.Object);
            // Assert
            Assert.Equal(expected.Object, actual);
        }

        [Fact]
        public void CreateWhenSpecificationIsSatisfiedButRequestIsNotTypeReturnsCorrectResult()
        {
            // Arrange
            var specificationStub = new Mock<IRequestSpecification>();
            specificationStub
                .Setup(s => s.IsSatisfiedBy(It.IsAny<object>()))
                .Returns(true);

            var sut = new MockRelay(specificationStub.Object);

            var request = new object();
            // Act
            var dummyContext = new Mock<ISpecimenContext>().Object;
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }
    }
}
