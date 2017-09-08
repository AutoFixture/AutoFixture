using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultipleRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MultipleRequest(null));
            // Teardown
        }

        [Fact]
        public void RequestIsCorrect()
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new MultipleRequest(expectedRequest);
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
            var sut = new MultipleRequest(new object());
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<MultipleRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new MultipleRequest(new object());
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
            var sut = new MultipleRequest(new object());
            MultipleRequest other = null;
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
            var sut = new MultipleRequest(new object());
            object anonymousObject = new ConcreteType();
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
            var sut = new MultipleRequest(new object());
            object other = new MultipleRequest(new object());
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
            var sut = new MultipleRequest(new object());
            var other = new MultipleRequest(new object());
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
            var sut = new MultipleRequest(request);
            object other = new MultipleRequest(request);
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
            var sut = new MultipleRequest(request);
            var other = new MultipleRequest(request);
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
            var sut = new MultipleRequest(new object());
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedResult = sut.Request.GetHashCode();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
