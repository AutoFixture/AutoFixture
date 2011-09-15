using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ConstrainedStringRequestTest
    {
        [Fact]
        public void MaximumIsCorrect()
        {
            // Fixture setup
            var expectedMaximum = 3;
            var sut = new ConstrainedStringRequest(expectedMaximum);
            // Exercise system
            var result = sut.Maximum;
            // Verify outcome
            Assert.Equal(expectedMaximum, result);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void CreateWithMaximumLowerThanOneWillThrow(int maximum)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringRequest(maximum));
            // Teardown
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithMaximumEqualsOrGreaterThanOneWillDoesNotThrow(int maximum)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => new ConstrainedStringRequest(maximum));
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
        public void SutDoesNotEqualOtherObjectWhenMaximumsDiffer()
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
        public void SutDoesNotEqualOtherSutWhenMaximumsDiffer()
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
            int maximum = 3;
            var sut = new ConstrainedStringRequest(maximum);
            object other = new ConstrainedStringRequest(maximum);
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
            int maximum = 3;
            var sut = new ConstrainedStringRequest(maximum);
            var other = new ConstrainedStringRequest(maximum);
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
            int maximum = 3;
            var sut = new ConstrainedStringRequest(maximum);
            var expectedHashCode = maximum.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}