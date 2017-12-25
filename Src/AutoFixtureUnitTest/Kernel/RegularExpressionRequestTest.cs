using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class RegularExpressionRequestTest
    {
        [Fact]
        public void CreateWithNullPatternThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RegularExpressionRequest(null));
        }

        [Theory]
        [InlineData("[0-9]")]
        [InlineData("[A-Z]")]
        [InlineData("[a-z]")]
        public void PatternIsCorrect(string pattern)
        {
            // Arrange
            var sut = new RegularExpressionRequest(pattern);
            // Act
            var result = sut.Pattern;
            // Assert
            Assert.Equal(pattern, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new RegularExpressionRequest("[0-9]");
            // Assert
            Assert.IsAssignableFrom<IEquatable<RegularExpressionRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            object other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            RegularExpressionRequest other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            object anonymousObject = new ConcreteType();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenPatternsAreDifferent()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            object other = new RegularExpressionRequest("[A-Z]");
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenPatternsAreDifferent()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            var other = new RegularExpressionRequest("[A-Z]");
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenPatternsMatch()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            object other = new RegularExpressionRequest("[0-9]");
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenPatternsMatch()
        {
            // Arrange
            var sut = new RegularExpressionRequest("[0-9]");
            var other = new RegularExpressionRequest("[0-9]");
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            string pattern = "[0-9]";
            var sut = new RegularExpressionRequest(pattern);
            var expectedHashCode = pattern.GetHashCode();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(expectedHashCode, result);
        }
    }
}