using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ConstrainedStringRequestTest
    {
        [Fact]
        public void MaximumLengthIsCorrect()
        {
            // Fixture setup
            var expectedMaximumLength = 3;
            var sut = new ConstrainedStringRequest(expectedMaximumLength);
            // Exercise system
            var result = sut.MaximumLength;
            // Verify outcome
            Assert.Equal(expectedMaximumLength, result);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void CreateWithMaximumLengthLowerThanOneWillThrow(int maximumLength)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(maximumLength));
            // Teardown
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithMaximumLengthEqualsOrGreaterThanOneWillDoesNotThrow(int maximumLength)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => new ConstrainedStringRequest(maximumLength));
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
            object anonymousObject = new FileStyleUriParser();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMaximumLengthsDiffer()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(3);
            object other = new ConstrainedStringRequest(4);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMaximumLengthsDiffer()
        {
            // Fixture setup
            var sut = new ConstrainedStringRequest(3);
            var other = new ConstrainedStringRequest(4);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenConstructorParametersEquals()
        {
            // Fixture setup
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(maximumLength);
            object other = new ConstrainedStringRequest(maximumLength);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenConstructorParametersEquals()
        {
            // Fixture setup
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(maximumLength);
            var other = new ConstrainedStringRequest(maximumLength);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void GetHashCodeWillReturnCorrectResult()
        {
            // Fixture setup
            int maximumLength = 3;
            var sut = new ConstrainedStringRequest(maximumLength);
            var expectedHashCode = maximumLength.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}