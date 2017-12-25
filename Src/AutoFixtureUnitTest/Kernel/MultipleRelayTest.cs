using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MultipleRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new MultipleRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CountIsProperWritableProperty()
        {
            // Arrange
            var sut = new MultipleRelay();
            var expectedCount = 1;
            // Act
            sut.Count = expectedCount;
            int result = sut.Count;
            // Assert
            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public void DefaultCountIsCorrect()
        {
            // Arrange
            var sut = new MultipleRelay();
            // Act
            var result = sut.Count;
            // Assert
            Assert.Equal(3, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-21)]
        public void SettingInvalidCountThrows(int count)
        {
            // Arrange
            var sut = new MultipleRelay();
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                sut.Count = count);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new MultipleRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MultipleRelay();
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
            var sut = new MultipleRelay();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithManyRequestReturnsCorrectResult()
        {
            // Arrange
            var request = new MultipleRequest(new object());
            var count = 7;
            var expectedTranslation = new FiniteSequenceRequest(request.Request, 7);
            var expectedResult = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedTranslation.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new MultipleRelay { Count = count };
            // Act
            var result = sut.Create(request, container);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
