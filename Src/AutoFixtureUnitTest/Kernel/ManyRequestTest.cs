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
    }
}
