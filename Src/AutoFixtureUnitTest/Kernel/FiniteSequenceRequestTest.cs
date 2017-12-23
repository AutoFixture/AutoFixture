using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class FiniteSequenceRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestAndValidNumberThrows()
        {
            // Arrange
            var dummyNumber = 1;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FiniteSequenceRequest(null, dummyNumber));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-3)]
        public void InitializeWithInvalidCountThrows(int count)
        {
            // Arrange
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new FiniteSequenceRequest(dummyRequest, count));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        public void CreateRequestsReturnsCorrectResult(int requestedCount)
        {
            // Arrange
            var expectedRequest = new object();
            var sut = new FiniteSequenceRequest(expectedRequest, requestedCount);
            // Act
            var result = sut.CreateRequests().ToList();
            // Assert
            Assert.Equal(requestedCount, result.Count);
            Assert.True(result.All(expectedRequest.Equals));
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            var dummyCount = 3;
            // Act
            var sut = new FiniteSequenceRequest(new object(), dummyCount);
            // Assert
            Assert.IsAssignableFrom<IEquatable<FiniteSequenceRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new FiniteSequenceRequest(new object(), 3);
            object other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var sut = new FiniteSequenceRequest(new object(), 3);
            FiniteSequenceRequest other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new FiniteSequenceRequest(new object(), 3);
            object anonymousObject = new ConcreteType();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRequestsDifferButCountsMatch()
        {
            // Arrange
            var count = 1;
            var sut = new FiniteSequenceRequest(new object(), count);
            object other = new FiniteSequenceRequest(new object(), count);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenRequestsDifferButCountsMatch()
        {
            // Arrange
            var count = 1;
            var sut = new FiniteSequenceRequest(new object(), count);
            var other = new FiniteSequenceRequest(new object(), count);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenCountsDiffer()
        {
            // Arrange
            var request = new object();
            var sut = new FiniteSequenceRequest(request, 1);
            object other = new FiniteSequenceRequest(request, 2);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenCountsDiffer()
        {
            // Arrange
            var request = new object();
            var sut = new FiniteSequenceRequest(request, 1);
            var other = new FiniteSequenceRequest(request, 2);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothRequestsAndCountMatch()
        {
            // Arrange
            var request = new object();
            var count = 4;
            var sut = new FiniteSequenceRequest(request, count);
            object other = new FiniteSequenceRequest(request, count);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothRequestsAndCountMatch()
        {
            // Arrange
            var request = new object();
            var count = 4;
            var sut = new FiniteSequenceRequest(request, count);
            var other = new FiniteSequenceRequest(request, count);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeWhenCountIsSpecifiedReturnsCorrectResult()
        {
            // Arrange
            var request = new object();
            var count = 19;
            var sut = new FiniteSequenceRequest(request, count);
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedHashCode = request.GetHashCode() ^ count.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }
    }
}
