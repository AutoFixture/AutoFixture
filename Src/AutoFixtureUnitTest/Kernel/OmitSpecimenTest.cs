using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class OmitSpecimenTest
    {
        [Fact]
        public void SutIsEquatable()
        {
            var sut = new OmitSpecimen();
            Assert.IsAssignableFrom<IEquatable<OmitSpecimen>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNull()
        {
            // Arrange
            var sut = new OmitSpecimen();
            // Act
            var actual = BothEquals(sut, null);
            // Assert
            Assert.DoesNotContain(actual, b => b);
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new OmitSpecimen();
            // Act
            var anonymous = new object();
            var actual = sut.Equals(anonymous);
            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void SutEqualsOtherSut()
        {
            // Arrange
            var sut = new OmitSpecimen();
            var other = new OmitSpecimen();
            // Act
            var actual = BothEquals(sut, other);
            // Assert
            Assert.True(actual.All(b => b));
        }

        [Fact]
        public void GetHashCodeIsStableAcrossInstances()
        {
            // Arrange
            var sut = new OmitSpecimen();
            // Act
            var actual = sut.GetHashCode();
            // Assert
            var expected = new OmitSpecimen().GetHashCode();
            Assert.Equal(expected, actual);
        }

        private static IEnumerable<bool> BothEquals<T>(T sut, T other) where T : IEquatable<T>
        {
            yield return sut.Equals((object)other);
            yield return sut.Equals(other);
        }
    }
}
