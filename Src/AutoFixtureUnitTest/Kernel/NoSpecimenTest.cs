using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenTest
    {
        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = NoSpecimen.Instance;
            // Assert
            Assert.IsAssignableFrom<IEquatable<NoSpecimen>>(sut);
        }

        [Fact]
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "This test asserts the result of the custom Equals method")]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = NoSpecimen.Instance;
            object other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "This test asserts the result of the custom Equals method")]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var sut = NoSpecimen.Instance;
            NoSpecimen other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = NoSpecimen.Instance;
            var anonymousObject = new object();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObject()
        {
            // Arrange
            var sut = NoSpecimen.Instance;
            object other = NoSpecimen.Instance;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSut()
        {
            // Arrange
            var sut = NoSpecimen.Instance;
            var other = NoSpecimen.Instance;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeWillAlwaysReturnZeroResult()
        {
            // Arrange
            var sut = NoSpecimen.Instance;
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(0, result);
        }
    }
}
