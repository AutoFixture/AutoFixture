using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class EmailAddressLocalPartTest
    {
        [Fact]
        public void InitializeWithNullLocalPartThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new EmailAddressLocalPart(null));
        }

        [Fact]
        public void InitializeWithEmptyLocalPartThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(
                () => new EmailAddressLocalPart(string.Empty));
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Arrange
            string expected = Guid.NewGuid().ToString();
            var sut = new EmailAddressLocalPart(expected);
            // Act
            var result = sut.ToString();
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "This test asserts the result of the custom Equals method")]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            object other = null;
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            var anonymousObject = new object();
            // Act
            bool result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenLocalPartsDiffer()
        {
            // Arrange
            var sut = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            object other = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenLocalPartsAreEqual()
        {
            // Arrange
            var localPart = Guid.NewGuid().ToString();

            var sut = new EmailAddressLocalPart(localPart);
            var other = new EmailAddressLocalPart(localPart);
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            var localPart = Guid.NewGuid().ToString();
            var sut = new EmailAddressLocalPart(localPart);
            // Act
            int result = sut.GetHashCode();
            // Assert
            int expectedHashCode = HashCode.Combine(localPart);
            Assert.Equal(expectedHashCode, result);
        }
    }
}
