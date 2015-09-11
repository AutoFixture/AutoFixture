using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class CriterionTests
    {
        [Fact]
        public void SutIsEquatable()
        {
            var sut = new Criterion<string>(
                "ploeh",
                new DelegatingEqualityComparer<string>());
            Assert.IsAssignableFrom<IEquatable<string>>(sut);
        }

        [Fact]
        public void ConstructWithNullComparerThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Criterion<int>(42, null));
        }

        [Theory]
        [InlineData("ploeh", "ndøh", true)]
        [InlineData("fnaah", "sqryt", false)]
        [InlineData(null, "qux", true)]
        public void EqualsReturnsComparerResult(
            string target,
            string candidate,
            bool expected)
        {
            var comparer = new DelegatingEqualityComparer<string>
            {
                OnEquals = (x, y) =>
                {
                    Assert.Equal(target, x);
                    Assert.Equal(candidate, y);
                    return expected;
                }
            };
            var sut = new Criterion<string>(target, comparer);

            var actual = sut.Equals(candidate);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(42)]
        public void TargetIsCorrect(int expected)
        {
            var sut = new Criterion<int>(
                expected,
                new DelegatingEqualityComparer<int>());
            var actual = sut.Target;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            var expected = new DelegatingEqualityComparer<decimal>();
            var sut = new Criterion<decimal>(1337m, expected);

            var actual = sut.Comparer;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SutEqualsIdenticalValue()
        {
            var target = Guid.NewGuid();
            var comparer = new DelegatingEqualityComparer<Guid>();
            var sut = new Criterion<Guid>(target, comparer);

            var other = new Criterion<Guid>(target, comparer);
            var actual = sut.Equals(other);

            Assert.True(actual, "Expected structural equality to hold.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        [InlineData(typeof(Version))]
        public void SutDoesNotEqualAnyObject(object other)
        {
            var sut = new Criterion<PlatformID>(
                PlatformID.Unix,
                new DelegatingEqualityComparer<PlatformID>());
            var actual = sut.Equals(other);
            Assert.False(actual, "SUT should not equal object of other type.");
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 36)]
        public void SutDoesNotEqualOtherWhenTargetDiffers(
            int sutTarget,
            int otherTarget)
        {
            var comparer = new DelegatingEqualityComparer<int>();
            var sut = new Criterion<int>(sutTarget, comparer);

            var other = new Criterion<int>(otherTarget, comparer);
            var actual = sut.Equals(other);

            Assert.False(
                actual,
                "SUT shouldn't equal other with different target");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("3")]
        [InlineData("foo")]
        public void SutDoesNotEqualOtherWhenComparerDiffers(string target)
        {
            var sut = new Criterion<string>(
                target,
                new DelegatingEqualityComparer<string>());

            var other = new Criterion<string>(
                target,
                new DelegatingEqualityComparer<string>());
            var actual = sut.Equals(other);

            Assert.False(
                actual,
                "SUT shouldn't equal other with different comparer.");
        }
    }
}
