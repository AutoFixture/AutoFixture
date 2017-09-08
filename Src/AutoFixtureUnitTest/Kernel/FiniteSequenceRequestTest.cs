using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FiniteSequenceRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestAndValidNumberThrows()
        {
            // Fixture setup
            var dummyNumber = 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FiniteSequenceRequest(null, dummyNumber));
            // Teardown
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-3)]
        public void InitializeWithInvalidCountThrows(int count)
        {
            // Fixture setup
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new FiniteSequenceRequest(dummyRequest, count));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        public void CreateRequestsReturnsCorrectResult(int requestedCount)
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new FiniteSequenceRequest(expectedRequest, requestedCount);
            // Exercise system
            var result = sut.CreateRequests().ToList();
            // Verify outcome
            Assert.Equal(requestedCount, result.Count);
            Assert.True(result.All(expectedRequest.Equals));
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var dummyCount = 3;
            // Exercise system
            var sut = new FiniteSequenceRequest(new object(), dummyCount);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<FiniteSequenceRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new FiniteSequenceRequest(new object(), 3);
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
            var sut = new FiniteSequenceRequest(new object(), 3);
            FiniteSequenceRequest other = null;
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
            var sut = new FiniteSequenceRequest(new object(), 3);
            object anonymousObject = new ConcreteType();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRequestsDifferButCountsMatch()
        {
            // Fixture setup
            var count = 1;
            var sut = new FiniteSequenceRequest(new object(), count);
            object other = new FiniteSequenceRequest(new object(), count);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenRequestsDifferButCountsMatch()
        {
            // Fixture setup
            var count = 1;
            var sut = new FiniteSequenceRequest(new object(), count);
            var other = new FiniteSequenceRequest(new object(), count);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenCountsDiffer()
        {
            // Fixture setup
            var request = new object();
            var sut = new FiniteSequenceRequest(request, 1);
            object other = new FiniteSequenceRequest(request, 2);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenCountsDiffer()
        {
            // Fixture setup
            var request = new object();
            var sut = new FiniteSequenceRequest(request, 1);
            var other = new FiniteSequenceRequest(request, 2);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothRequestsAndCountMatch()
        {
            // Fixture setup
            var request = new object();
            var count = 4;
            var sut = new FiniteSequenceRequest(request, count);
            object other = new FiniteSequenceRequest(request, count);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothRequestsAndCountMatch()
        {
            // Fixture setup
            var request = new object();
            var count = 4;
            var sut = new FiniteSequenceRequest(request, count);
            var other = new FiniteSequenceRequest(request, count);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenCountIsSpecifiedReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var count = 19;
            var sut = new FiniteSequenceRequest(request, count);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHashCode = request.GetHashCode() ^ count.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}
