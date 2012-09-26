using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomNumericSequenceLimitTest
    {
        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => new RandomNumericSequenceLimit());
            // Teardown
        }

        [Fact]
        public void InitializeWithDefaultConstructorSetsCorrectLimits()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceLimit();
            var expectedResult = new[]
            {
                Byte.MaxValue,
                Int16.MaxValue,
                Int32.MaxValue
            };
            // Exercise system
            IEnumerable<int> result = sut.Limit;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            IEnumerable<int> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceLimit(nullEnumerable));
            // Teardown
        }

        [Fact]
        public void LimitMatchListParameter()
        {
            // Fixture setup
            var expectedResult = new[] { 10, 20, 30 }.AsEnumerable();
            var sut = new RandomNumericSequenceLimit(expectedResult);
            // Exercise system
            IEnumerable<int> result = sut.Limit;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(new[] { -3,  5,  9 })]
        [InlineData(new[] {  3, -5,  9 })]
        [InlineData(new[] {  3,  5, -9 })]
        [InlineData(new[] {  1,  5,  9 })]
        public void InitializeWithInvalidUpperLimitListParameterThrows(int[] upperLimits)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new RandomNumericSequenceLimit(upperLimits.AsEnumerable()));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            int[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceLimit(nullArray));
            // Teardown
        }

        [Fact]
        public void LimitMatchParamsArray()
        {
            // Fixture setup
            var expectedResult = new[] { 10, 20, 30 };
            var sut = new RandomNumericSequenceLimit(expectedResult);
            // Exercise system
            IEnumerable<int> result = sut.Limit;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(new[] { -3,  5,  9 })]
        [InlineData(new[] {  3, -5,  9 })]
        [InlineData(new[] {  3,  5, -9 })]
        [InlineData(new[] {  1,  5,  9 })]
        public void InitializeWithInvalidUpperLimitParamsArrayThrows(int[] upperLimits)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new RandomNumericSequenceLimit(upperLimits));
            // Teardown
        }

        [Fact]
        public void LimitDoesNotMatchParamsArray()
        {
            // Fixture setup
            var unexpectedLimit = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue
            };
            var sut = new RandomNumericSequenceLimit(
                unexpectedLimit[0],
                unexpectedLimit[2],
                unexpectedLimit[1]
                );
            // Exercise system
            IEnumerable<int> result = sut.Limit;
            // Verify outcome
            Assert.False(unexpectedLimit.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomNumericSequenceLimit();
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<RandomNumericSequenceLimit>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNull()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceLimit();
            // Exercise system
            IEnumerable<bool> actual = BothEquals(sut, null);
            // Verify outcome
            Assert.False(actual.Any(b => b));
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceLimit();
            // Exercise system
            var anonymous = new object();
            bool actual = sut.Equals(anonymous);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSut()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceLimit();
            var other = new RandomNumericSequenceLimit();
            // Exercise system
            var actual = BothEquals(sut, other);
            // Verify outcome
            Assert.True(actual.All(b => b));
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenLimitsDiffer()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceLimit(2, 5, 9);
            var other = new RandomNumericSequenceLimit(3, 6, 8);
            // Exercise system
            var actual = BothEquals(sut, other);
            // Verify outcome
            Assert.False(actual.All(b => b));
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothLimitsAreEqual()
        {
            // Fixture setup
            var upperLimits = new[] { 10, 20, 30 };
            var sut = new RandomNumericSequenceLimit(upperLimits);
            var other = new RandomNumericSequenceLimit(upperLimits);
            // Exercise system
            var actual = BothEquals(sut, other);
            // Verify outcome
            Assert.True(actual.All(b => b));
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenLimitIsDefaultReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceLimit();
            var expectedResult = sut.Limit.GetHashCode();
            // Exercise system
            int result = sut.GetHashCode(); 
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenLimitIsNotDefaultReturnsCorrectResult()
        {
            // Fixture setup
            var upperLimits = new[] { Byte.MaxValue, Int16.MaxValue, Int32.MaxValue };
            var expectedResult = upperLimits.GetHashCode();
            var sut = new RandomNumericSequenceLimit(upperLimits);
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private static IEnumerable<bool> BothEquals<T>(T sut, T other) where T : IEquatable<T>
        {
            yield return sut.Equals((object)other);
            yield return sut.Equals(other);
        }
    }
}
