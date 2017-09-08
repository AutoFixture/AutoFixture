using System;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UriSchemeTest
    {
        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => new UriScheme()));
            // Teardown
        }

        [Fact]
        public void InitializeWithDefaultConstructorSetsCorrectScheme()
        {
            // Fixture setup
            var sut = new UriScheme();
            string expectedScheme = "http";
            // Exercise system
            string result = sut.Scheme;
            // Verify outcome
            Assert.Equal(expectedScheme, result);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSchemeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new UriScheme(null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData("scheme:")]
        [InlineData("scheme/")]
        [InlineData("scheme:/")]
        public void InitializeWithInvalidSchemeThrows(string scheme)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(
                () => new UriScheme(scheme));
            // Teardown
        }

        [Fact]
        public void InitializeWithSchemeParameterSetsCorrectScheme()
        {
            // Fixture setup
            string expectedScheme = "http";
            var sut = new UriScheme("http");
            // Exercise system
            string result = sut.Scheme;
            // Verify outcome
            Assert.Equal(expectedScheme, result);
            // Teardown
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            string expected = "http";
            var sut = new UriScheme("http");
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new UriScheme();
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<UriScheme>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new UriScheme();
            object other = null;
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = new UriScheme();
            UriScheme other = null;
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new UriScheme();
            var anonymousObject = new object();
            // Exercise system
            bool result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenSchemesDiffer()
        {
            // Fixture setup
            var sut = new UriScheme("a");
            object other = new UriScheme("b");
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenSchemesDiffer()
        {
            // Fixture setup
            var sut = new UriScheme("a");
            var other = new UriScheme("b");
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothSchemesAreDefault()
        {
            // Fixture setup
            var sut = new UriScheme();
            object other = new UriScheme();
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothSchemesAreDefault()
        {
            // Fixture setup
            var sut = new UriScheme();
            var other = new UriScheme();
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenSchemesAreEqual()
        {
            // Fixture setup
            var scheme = "https";
            var sut = new UriScheme(scheme);
            object other = new UriScheme(scheme);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenSchemesAreEqual()
        {
            // Fixture setup
            var scheme = "https";
            var sut = new UriScheme(scheme);
            var other = new UriScheme(scheme);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenSchemeIsDefaultReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new UriScheme();
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            int expectedHashCode = "http".GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenSchemeIsNotDefaultReturnsCorrectResult()
        {
            // Fixture setup
            var scheme = "https";
            var sut = new UriScheme(scheme);
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            int expectedHashCode = scheme.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}