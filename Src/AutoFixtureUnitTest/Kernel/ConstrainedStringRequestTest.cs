using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ConstrainedStringRequestTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void CreateWithMinimumLengthLowerThanZeroWillThrow(int minimumLength)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(minimumLength, 3));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void CreateWithMaximumLengthLowerThanZeroWillThrow(int maximumLength)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(maximumLength));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public void CreateWithBiggerMinimumLengthThanMaximumLengthWillThrow(int minimumLength, int maximum)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(minimumLength, maximum));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void MinimumIsCorrect(int expectedMinimumLength)
        {
            // Arrange
            var sut = new ConstrainedStringRequest(expectedMinimumLength, 5);
            // Act
            var result = sut.MinimumLength;
            // Assert
            Assert.Equal(expectedMinimumLength, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void MaximumLengthIsCorrect(int expectedMaximumLength)
        {
            // Arrange
            var sut = new ConstrainedStringRequest(expectedMaximumLength);
            // Act
            var result = sut.MaximumLength;
            // Assert
            Assert.Equal(expectedMaximumLength, result);
        }

        [Fact]
        public void CreateWithMaximumLengthAssignsCorrectValueToMinimumLength()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(3);
            var expectedMinimumLength = 0;
            // Act
            var result = sut.MinimumLength;
            // Assert
            Assert.Equal(expectedMinimumLength, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new ConstrainedStringRequest(3);
            // Assert
            Assert.IsAssignableFrom<IEquatable<ConstrainedStringRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(3);
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
            var sut = new ConstrainedStringRequest(3);
            ConstrainedStringRequest other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(3);
            object anonymousObject = new ConcreteType();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMinimumLengthDifferButMaximumLengthsMatch()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(1, 3);
            object other = new ConstrainedStringRequest(2, 3);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMinimumLengthDifferButMaximumLengthsMatch()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(1, 3);
            var other = new ConstrainedStringRequest(2, 3);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMaximumLengthDifferButMinimumLengthsMatch()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(1, 3);
            object other = new ConstrainedStringRequest(1, 4);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMaximumLengthDifferButMinimumLengthsMatch()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(1, 3);
            var other = new ConstrainedStringRequest(1, 4);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothLengthsMatch()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(1, 5);
            object other = new ConstrainedStringRequest(1, 5);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothLengthsMatch()
        {
            // Arrange
            var sut = new ConstrainedStringRequest(1, 5);
            var other = new ConstrainedStringRequest(1, 5);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            int minimumLength = 0;
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(maximumLength);
            var expectedHashCode = minimumLength.GetHashCode() ^ maximumLength.GetHashCode();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void GetHashCodeWhenMinimumLengthIsSpecifiedReturnsCorrectResult()
        {
            // Arrange
            int minimumLength = 1;
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(minimumLength, maximumLength);
            var expectedHashCode = minimumLength.GetHashCode() ^ maximumLength.GetHashCode();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(expectedHashCode, result);
        }
    }
}