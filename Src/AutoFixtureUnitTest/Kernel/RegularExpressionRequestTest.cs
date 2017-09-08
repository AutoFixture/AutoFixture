using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class RegularExpressionRequestTest
    {
        [Fact]
        public void CreateWithNullPatternThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RegularExpressionRequest(null));
            // Teardown
        }

        [Theory]
        [InlineData("[0-9]")]
        [InlineData("[A-Z]")]
        [InlineData("[a-z]")]
        public void PatternIsCorrect(string pattern)
        {
            // Fixture setup
            var sut = new RegularExpressionRequest(pattern);
            // Exercise system
            var result = sut.Pattern;
            // Verify outcome
            Assert.Equal(pattern, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new RegularExpressionRequest("[0-9]");
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<RegularExpressionRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            object other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            RegularExpressionRequest other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            object anonymousObject = new ConcreteType();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenPatternsAreDifferent()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            object other = new RegularExpressionRequest("[A-Z]");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenPatternsAreDifferent()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            var other = new RegularExpressionRequest("[A-Z]");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenPatternsMatch()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            object other = new RegularExpressionRequest("[0-9]");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenPatternsMatch()
        {
            // Fixture setup
            var sut = new RegularExpressionRequest("[0-9]");
            var other = new RegularExpressionRequest("[0-9]");
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
            string pattern = "[0-9]";
            var sut = new RegularExpressionRequest(pattern);
            var expectedHashCode = pattern.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}