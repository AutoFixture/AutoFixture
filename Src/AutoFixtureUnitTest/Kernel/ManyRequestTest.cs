using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ManyRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestAndValidNumberThrows()
        {
            // Fixture setup
            var dummyNumber = 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ManyRequest(null, dummyNumber));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void InitializeWithInvalidCountThrows(int count)
        {
            // Fixture setup
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ManyRequest(dummyRequest, count));
            // Teardown
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        public void CreateRequestsReturnsCorrectResult(int requestedCount)
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new ManyRequest(expectedRequest, requestedCount);
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
            var sut = new ManyRequest(new object(), dummyCount);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<ManyRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new ManyRequest(new object(), 3);
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
            var sut = new ManyRequest(new object(), 3);
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
            var sut = new ManyRequest(new object(), 3);
            object anonymousObject = new FileStyleUriParser();
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
            var sut = new ManyRequest(new object(), count);
            object other = new ManyRequest(new object(), count);
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
            var sut = new ManyRequest(new object(), count);
            var other = new ManyRequest(new object(), count);
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
            var sut = new ManyRequest(request, 1);
            object other = new ManyRequest(request, 2);
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
            var sut = new ManyRequest(request, 1);
            var other = new ManyRequest(request, 2);
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
            var sut = new ManyRequest(request, count);
            object other = new ManyRequest(request, count);
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
            var sut = new ManyRequest(request, count);
            var other = new ManyRequest(request, count);
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
            var sut = new ManyRequest(request, count);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHashCode = request.GetHashCode() ^ count.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}
