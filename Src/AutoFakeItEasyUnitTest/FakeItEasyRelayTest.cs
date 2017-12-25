using System;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new FakeItEasyRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyRelay(null));
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Arrange
            var expected = new Fake<IRequestSpecification>().FakedObject;
            var sut = new FakeItEasyRelay(expected);
            // Act
            IRequestSpecification result = sut.FakeableSpecification;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new FakeItEasyRelay();
            // Act
            var result = sut.FakeableSpecification;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new FakeItEasyRelay();
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
            var sut = new FakeItEasyRelay();
            var dummyContext = new Fake<ISpecimenContext>().FakedObject;
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
            var fakeType = typeof(Fake<>).MakeGenericType(request);

            var fake = Activator.CreateInstance(fakeType);
            var contextStub = new Fake<ISpecimenContext>();
            contextStub.CallsTo(ctx => ctx.Resolve(fakeType)).Returns(fake);

            var sut = new FakeItEasyRelay();
            // Act
            var result = sut.Create(request, contextStub.FakedObject);
            // Assert
            var expected = fake.GetType().GetProperty("FakedObject").GetValue(fake, null);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonFake()
        {
            // Arrange
            var request = typeof(IInterface);
            var fakeType = typeof(Fake<>).MakeGenericType(request);

            var contextStub = new Fake<ISpecimenContext>();
            contextStub.CallsTo(ctx => ctx.Resolve(fakeType)).Returns(new object());

            var sut = new FakeItEasyRelay();
            // Act
            var result = sut.Create(request, contextStub.FakedObject);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWhenSpecificationIsSatisfiedReturnsCorrectResult(
            Type request)
        {
            // Arrange
            var specificationStub = A.Fake<IRequestSpecification>();
            A.CallTo(() => specificationStub.IsSatisfiedBy(request))
                .Returns(true);

            var expected = typeof(Fake<>)
                .MakeGenericType(request)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);
            var contextStub = A.Fake<ISpecimenContext>();
            A.CallTo(() => contextStub.Resolve(typeof(Fake<>).MakeGenericType(request)))
                .Returns(expected);

            var sut = new FakeItEasyRelay(specificationStub);
            // Act
            var actual = sut.Create(request, contextStub);
            // Assert
            Assert.Equal(
                expected.GetType().GetProperty("FakedObject").GetValue(expected, null),
                actual);
        }

        [Fact]
        public void CreateWhenSpecificationIsSatisfiedButRequestIsNotTypeReturnsCorrectResult()
        {
            // Arrange
            var specificationStub = A.Fake<IRequestSpecification>();
            A.CallTo(() => specificationStub.IsSatisfiedBy(A<object>.Ignored))
                .Returns(true);

            var sut = new FakeItEasyRelay(specificationStub);

            var request = new object();
            // Act
            var dummyContext = A.Fake<ISpecimenContext>();
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }
    }
}
