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
            // Fixture setup
            var dummyTarget = new object();
            // Exercise system
            var sut = new EqualRequestSpecification(dummyTarget);
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void TargetIsCorrect()
        {
            // Fixture setup
            var expected = new object();
            var sut = new EqualRequestSpecification(expected);
            // Exercise system
            var actual = sut.Target;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByTargetReturnsCorrectResult()
        {
            // Fixture setup
            var target = new object();
            var sut = new EqualRequestSpecification(target);
            // Exercise system
            var actual = sut.IsSatisfiedBy(target);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByOtherReturnsCorrectResult()
        {
            // Fixture setup
            var dummyTarget = new object();
            var sut = new EqualRequestSpecification(dummyTarget);
            // Exercise system
            var other = new object();
            var actual = sut.IsSatisfiedBy(other);
            // Verify outcome
            Assert.False(actual);
            // Teardown
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
            // Fixture setup
            var target = new object();
            var other = new object();
            var comparer = new DelegatingEqualityComparer
            {
                OnEquals = (x, y) => x == target && y == other && expected
            };
            var sut = new EqualRequestSpecification(target, comparer);
            // Exercise system
            var actual = sut.IsSatisfiedBy(other);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullTargetAndDummyComparerThrows()
        {
            // Fixture setup
            var dummyComparer = new DelegatingEqualityComparer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new EqualRequestSpecification(null, dummyComparer));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullComparerAndDummyTargetThrows()
        {
            // Fixture setup
            var dummyTarget = new object();
            // Exercise system and verify outcome

            Assert.Throws<ArgumentNullException>(() =>
                new EqualRequestSpecification(dummyTarget, null));
            // Teardown
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingEqualityComparer();
            var dummyTarget = new object();
            var sut = new EqualRequestSpecification(dummyTarget, expected);
            // Exercise system
            IEqualityComparer actual = sut.Comparer;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}
