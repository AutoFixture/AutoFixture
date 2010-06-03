using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ManyRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ManyRequest(null));
            // Teardown
        }

        [Fact]
        public void RequestIsCorrect()
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new ManyRequest(expectedRequest);
            // Exercise system
            var result = sut.Request;
            // Verify outcome
            Assert.Equal(expectedRequest, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new ManyRequest(new object());
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<ManyRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new ManyRequest(new object());
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
            var sut = new ManyRequest(new object());
            ManyRequest other = null;
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
            var sut = new ManyRequest(new object());
            object anonymousObject = new FileStyleUriParser();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRequestDiffers()
        {
            // Fixture setup
            var sut = new ManyRequest(new object());
            object other = new ManyRequest(new object());
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenRequestDiffers()
        {
            // Fixture setup
            var sut = new ManyRequest(new object());
            var other = new ManyRequest(new object());
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenRequestMatches()
        {
            // Fixture setup
            var request = new object();
            var sut = new ManyRequest(request);
            object other = new ManyRequest(request);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenRequestMatches()
        {
            // Fixture setup
            var request = new object();
            var sut = new ManyRequest(request);
            var other = new ManyRequest(request);
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
            var sut = new ManyRequest(new object());
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedResult = sut.Request.GetHashCode();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
