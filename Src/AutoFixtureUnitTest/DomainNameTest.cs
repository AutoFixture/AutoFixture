﻿using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DomainNameTest
    {
        [Fact]
        public void InitializeWithNullDomainNameThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new DomainName(null));
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Arrange
            var expected = Guid.NewGuid().ToString();
            var sut = new DomainName(expected);
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
            var sut = new DomainName(Guid.NewGuid().ToString());
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
            var sut = new DomainName(Guid.NewGuid().ToString());
            var anonymousObject = new object();
            // Act
            bool result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenDomainNamesDiffer()
        {
            // Arrange
            var sut = new DomainName(Guid.NewGuid().ToString());
            object other = new DomainName(Guid.NewGuid().ToString());
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenDomainNamesAreEqual()
        {
            // Arrange
            var domainName = Guid.NewGuid().ToString();

            var sut = new DomainName(domainName);
            var other = new DomainName(domainName);
            // Act
            bool result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            var domainName = Guid.NewGuid().ToString();
            var sut = new DomainName(domainName);
            // Act
            int result = sut.GetHashCode();
            // Assert
            int expectedHashCode = HashCode.Combine(domainName);
            Assert.Equal(expectedHashCode, result);
        }
    }
}