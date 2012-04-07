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
            Assert.DoesNotThrow(() => new UriScheme());
            // Teardown
        }

        [Fact]
        public void InitializeWithDefaultConstructorSetsCorrectScheme()
        {
            // Fixture setup
            var sut = new UriScheme();
            string expectedScheme = "scheme";
            // Exercise system
            string result = sut.Scheme;
            // Verify outcome
            Assert.Equal(expectedScheme, result);
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void InitializeWithNullOrEmptySchemeThrows(string scheme)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new UriScheme(scheme));
            // Teardown
        }

        [Theory]
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
    }
}
