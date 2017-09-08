using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ConstrainedStringRequestTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void CreateWithMinimumLengthLowerThanZeroWillThrow(int minimumLength)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(minimumLength, 3));
            // Teardown
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void CreateWithMaximumLengthLowerThanZeroWillThrow(int maximumLength)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(maximumLength));
            // Teardown
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public void CreateWithBiggerMinimumLengthThanMaximumLengthWillThrow(int minimumLength, int maximum)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(minimumLength, maximum));
            // Teardown
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void MinimumIsCorrect(int expectedMinimumLength)
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(expectedMinimumLength, 5);
            // Exercise system
            var result = sut.MinimumLength;
            // Verify outcome
            Assert.Equal(expectedMinimumLength, result);
            // Teardown
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void MaximumLengthIsCorrect(int expectedMaximumLength)
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(expectedMaximumLength);
            // Exercise system
            var result = sut.MaximumLength;
            // Verify outcome
            Assert.Equal(expectedMaximumLength, result);
            // Teardown
        }

        [Fact]
        public void CreateWithMaximumLengthAssignsCorrectValueToMinimumLength()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(3);
            var expectedMinimumLength = 0;
            // Exercise system
            var result = sut.MinimumLength;
            // Verify outcome
            Assert.Equal(expectedMinimumLength, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new ConstrainedStringRequest(3);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<ConstrainedStringRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(3);
            object other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(3);
            ConstrainedStringRequest other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(3);
            object anonymousObject = new ConcreteType();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMinimumLengthDifferButMaximumLengthsMatch()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(1, 3);
            object other = new ConstrainedStringRequest(2, 3);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMinimumLengthDifferButMaximumLengthsMatch()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(1, 3);
            var other = new ConstrainedStringRequest(2, 3);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMaximumLengthDifferButMinimumLengthsMatch()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(1, 3);
            object other = new ConstrainedStringRequest(1, 4);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMaximumLengthDifferButMinimumLengthsMatch()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(1, 3);
            var other = new ConstrainedStringRequest(1, 4);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothLengthsMatch()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(1, 5);
            object other = new ConstrainedStringRequest(1, 5);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothLengthsMatch()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(1, 5);
            var other = new ConstrainedStringRequest(1, 5);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Fixture setup
            int minimumLength = 0;
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(maximumLength);
            var expectedHashCode = minimumLength.GetHashCode() ^ maximumLength.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenMinimumLengthIsSpecifiedReturnsCorrectResult()
        {
            // Fixture setup
            int minimumLength = 1;
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(minimumLength, maximumLength);
            var expectedHashCode = minimumLength.GetHashCode() ^ maximumLength.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}