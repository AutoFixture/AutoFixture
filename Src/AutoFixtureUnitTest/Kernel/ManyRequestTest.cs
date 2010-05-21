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
        public void CreateWithNullRequestThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ManyRequest(null));
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestAndValidNumberThrows()
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
        public void CreateRequestsFromImplicitlyNumberedSutWithInvalidDefaultNumberThrows(int defaultNumber)
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new ManyRequest(dummyRequest);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                sut.CreateRequests(defaultNumber));
            // Teardown
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void CreateRequestsFromExplicitlyNumberedSutWithInvalidDefaultNumberThrows(int requestedNumber, int defaultNumber)
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new ManyRequest(dummyRequest, requestedNumber);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                sut.CreateRequests(defaultNumber));
            // Teardown
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void CreateRequestsFromImplicitlyNumberedSutReturnsCorrectResult(int defaultNumber)
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new ManyRequest(expectedRequest);
            // Exercise system
            var result = sut.CreateRequests(defaultNumber).ToList();
            // Verify outcome
            Assert.Equal(defaultNumber, result.Count);
            Assert.True(result.All(expectedRequest.Equals));
            // Teardown
        }

        [Theory]
        [InlineData(-1, 3, 3)]
        [InlineData(0, 3, 3)]
        [InlineData(1, 3, 1)]
        [InlineData(10, 3, 10)]
        [InlineData(3, -1, 3)]
        [InlineData(3, 0, 3)]
        public void CreateRequestsFromExplicitlyNumberedSutReturnsCorrectResult(int requestedNumber, int defaultNumber, int expectedNumber)
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new ManyRequest(expectedRequest, requestedNumber);
            // Exercise system
            var result = sut.CreateRequests(defaultNumber).ToList();
            // Verify outcome
            Assert.Equal(expectedNumber, result.Count);
            Assert.True(result.All(expectedRequest.Equals));
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
        public void SutDoesNotEqualOtherObjectWhenRequestsDiffer()
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
        public void SutDoesNotEqualOtherSutWhenRequestsDiffer()
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
        public void SutEqualsOtherObjectWhenRequestsMatch()
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
        public void SutEqualsOtherSutWhenRequestsMatch()
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
        public void GetHashCodeWhenCountIsNotSpecifiedReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var sut = new ManyRequest(request);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHashCode = request.GetHashCode();
            Assert.Equal(expectedHashCode, result);
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
