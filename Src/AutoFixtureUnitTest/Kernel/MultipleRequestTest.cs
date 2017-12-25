using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MultipleRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new MultipleRequest(null));
        }

        [Fact]
        public void RequestIsCorrect()
        {
            // Arrange
            var expectedRequest = new object();
            var sut = new MultipleRequest(expectedRequest);
            // Act
            var result = sut.Request;
            // Assert
            Assert.Equal(expectedRequest, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new MultipleRequest(new object());
            // Assert
            Assert.IsAssignableFrom<IEquatable<MultipleRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new MultipleRequest(new object());
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
            var sut = new MultipleRequest(new object());
            MultipleRequest other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new MultipleRequest(new object());
            object anonymousObject = new ConcreteType();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRequestDiffers()
        {
            // Arrange
            var sut = new MultipleRequest(new object());
            object other = new MultipleRequest(new object());
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenRequestDiffers()
        {
            // Arrange
            var sut = new MultipleRequest(new object());
            var other = new MultipleRequest(new object());
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenRequestMatches()
        {
            // Arrange
            var request = new object();
            var sut = new MultipleRequest(request);
            object other = new MultipleRequest(request);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenRequestMatches()
        {
            // Arrange
            var request = new object();
            var sut = new MultipleRequest(request);
            var other = new MultipleRequest(request);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            var sut = new MultipleRequest(new object());
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedResult = sut.Request.GetHashCode();
            Assert.Equal(expectedResult, result);
        }
    }
}
