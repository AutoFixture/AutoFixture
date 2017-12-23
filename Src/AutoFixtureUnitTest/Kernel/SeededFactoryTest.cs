using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SeededFactoryTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            Func<object, object> dummyFunc = obj => new object();
            // Act
            var sut = new SeededFactory<object>(dummyFunc);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SeededFactory<object>((Func<object, object>)null));
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Arrange
            Func<string, string> expectedResult = s => s;
            var sut = new SeededFactory<string>(expectedResult);
            // Act
            Func<string, string> result = sut.Factory;
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new SeededFactory<string>(s => s);
            var request = new object();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        public void CreateWithInvalidRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new SeededFactory<int>(s => s);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, "foo")]
        [InlineData(1, 1)]
        [InlineData(typeof(decimal), 1)]
        [InlineData(typeof(int), "foo")]
        [InlineData(typeof(decimal), null)]
        public void CreateWithInvalidSeededRequestReturnsCorrectResult(object request, object seed)
        {
            // Arrange
            var sut = new SeededFactory<int>(s => s);
            var seededRequest = new SeededRequest(request, seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(seededRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithCorrectSeedWillReturnCorrectResult()
        {
            // Arrange
            var seed = 7m;
            var seededRequest = new SeededRequest(typeof(decimal), seed);
            var expectedResult = 3m;

            Func<decimal, decimal> factoryStub = s => s == seed ? expectedResult : 0m;

            var sut = new SeededFactory<decimal>(factoryStub);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(seededRequest, dummyContext);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithCorrectlyTypedSeededRequestWithNullSeedReturnsCorrectResult()
        {
            // Arrange
            var seededRequest = new SeededRequest(typeof(string), null);
            var expectedResult = Guid.NewGuid().ToString();

            Func<string, string> factoryStub = s => s == null ? expectedResult : null;

            var sut = new SeededFactory<string>(factoryStub);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(seededRequest, dummyContext);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
