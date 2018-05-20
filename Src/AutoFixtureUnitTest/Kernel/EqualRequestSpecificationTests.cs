using System;
using System.Collections;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class EqualRequestSpecificationTests
    {
        [Fact]
        public void SutIsSpecification()
        {
            // Arrange
            var dummyTarget = new object();
            // Act
            var sut = new EqualRequestSpecification(dummyTarget);
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void TargetIsCorrect()
        {
            // Arrange
            var expected = new object();
            var sut = new EqualRequestSpecification(expected);
            // Act
            var actual = sut.Target;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsSatisfiedByTargetReturnsCorrectResult()
        {
            // Arrange
            var target = new object();
            var sut = new EqualRequestSpecification(target);
            // Act
            var actual = sut.IsSatisfiedBy(target);
            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSatisfiedByOtherReturnsCorrectResult()
        {
            // Arrange
            var dummyTarget = new object();
            var sut = new EqualRequestSpecification(dummyTarget);
            // Act
            var other = new object();
            var actual = sut.IsSatisfiedBy(other);
            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ConstructWithNullTargetThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new EqualRequestSpecification(null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSatisfiedByReturnsResultFromComparer(bool expected)
        {
            // Arrange
            var target = new object();
            var other = new object();
            var comparer = new DelegatingEqualityComparer
            {
                OnEquals = (x, y) => x == target && y == other && expected
            };
            var sut = new EqualRequestSpecification(target, comparer);
            // Act
            var actual = sut.IsSatisfiedBy(other);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullTargetAndDummyComparerThrows()
        {
            // Arrange
            var dummyComparer = new DelegatingEqualityComparer();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new EqualRequestSpecification(null, dummyComparer));
        }

        [Fact]
        public void ConstructWithNullComparerAndDummyTargetThrows()
        {
            // Arrange
            var dummyTarget = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new EqualRequestSpecification(dummyTarget, null));
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Arrange
            var expected = new DelegatingEqualityComparer();
            var dummyTarget = new object();
            var sut = new EqualRequestSpecification(dummyTarget, expected);
            // Act
            IEqualityComparer actual = sut.Comparer;
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
