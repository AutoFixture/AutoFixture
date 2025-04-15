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
            var sut = new NoSpecimen();
            // Assert
            Assert.IsAssignableFrom<IEquatable<NoSpecimen>>(sut);
        }

        [Fact]
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "This test asserts the result of the custom Equals method")]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new NoSpecimen();
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
            var sut = new NoSpecimen();
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
            var sut = new NoSpecimen();
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
            var sut = new NoSpecimen();
            object other = new NoSpecimen();
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSut()
        {
            // Arrange
            var sut = new NoSpecimen();
            var other = new NoSpecimen();
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeWillAlwaysReturnZeroResult()
        {
            // Arrange
            var sut = new NoSpecimen();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(0, result);
        }
    }
}
