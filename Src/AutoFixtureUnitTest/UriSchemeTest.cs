using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UriSchemeTest
    {
        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Arrange
            // Act & assert
            Assert.Null(Record.Exception(() => new UriScheme()));
        }

        [Fact]
        public void InitializeWithDefaultConstructorSetsCorrectScheme()
        {
            // Arrange
            var sut = new UriScheme();
            string expectedScheme = "http";
            // Act
            string result = sut.Scheme;
            // Assert
            Assert.Equal(expectedScheme, result);
        }

        [Fact]
        public void InitializeWithNullSchemeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new UriScheme(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("scheme:")]
        [InlineData("scheme/")]
        [InlineData("scheme:/")]
        public void InitializeWithInvalidSchemeThrows(string scheme)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(
                () => new UriScheme(scheme));
        }

        [Fact]
        public void InitializeWithSchemeParameterSetsCorrectScheme()
        {
            // Arrange
            string expectedScheme = "http";
            var sut = new UriScheme("http");
            // Act
            string result = sut.Scheme;
            // Assert
            Assert.Equal(expectedScheme, result);
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Arrange
            string expected = "http";
            var sut = new UriScheme("http");
            // Act
            var result = sut.ToString();
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new UriScheme();
            // Assert
            Assert.IsAssignableFrom<IEquatable<UriScheme>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new UriScheme();
            object other = null;
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var sut = new UriScheme();
            UriScheme other = null;
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new UriScheme();
            var anonymousObject = new object();
            // Act
            bool result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenSchemesDiffer()
        {
            // Arrange
            var sut = new UriScheme("a");
            object other = new UriScheme("b");
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenSchemesDiffer()
        {
            // Arrange
            var sut = new UriScheme("a");
            var other = new UriScheme("b");
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothSchemesAreDefault()
        {
            // Arrange
            var sut = new UriScheme();
            object other = new UriScheme();
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothSchemesAreDefault()
        {
            // Arrange
            var sut = new UriScheme();
            var other = new UriScheme();
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWhenSchemesAreEqual()
        {
            // Arrange
            var scheme = "https";
            var sut = new UriScheme(scheme);
            object other = new UriScheme(scheme);
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenSchemesAreEqual()
        {
            // Arrange
            var scheme = "https";
            var sut = new UriScheme(scheme);
            var other = new UriScheme(scheme);
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeWhenSchemeIsDefaultReturnsCorrectResult()
        {
            // Arrange
            var sut = new UriScheme();
            // Act
            int result = sut.GetHashCode();
            // Assert
            int expectedHashCode = "http".GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void GetHashCodeWhenSchemeIsNotDefaultReturnsCorrectResult()
        {
            // Arrange
            var scheme = "https";
            var sut = new UriScheme(scheme);
            // Act
            int result = sut.GetHashCode();
            // Assert
            int expectedHashCode = scheme.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }
    }
}