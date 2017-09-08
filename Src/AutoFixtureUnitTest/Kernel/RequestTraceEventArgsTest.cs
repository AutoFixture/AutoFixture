using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class RequestTraceEventArgsTest
    {
        [Fact]
        public void SutIsEventArgs()
        {
            // Fixture setup
            var dummyRequest = new object();
            var dummyDepth = 0;
            // Exercise system
            var sut = new RequestTraceEventArgs(dummyRequest, dummyDepth);
            // Verify outcome
            Assert.IsAssignableFrom<EventArgs>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorExplicitlyAllowsTracingNullRequestsIfThatShouldEverBeWarranted()
        {
            // Fixture setup
            var dummyDepth = 0;
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => new RequestTraceEventArgs(null, dummyDepth)));
            // Teardown
        }

        [Fact]
        public void RequestMatchesConstructorArgument()
        {
            // Fixture setup
            var expectedRequest = new object();
            var dummyDepth = 0;
            var sut = new RequestTraceEventArgs(expectedRequest, dummyDepth);
            // Exercise system
            var result = sut.Request;
            // Verify outcome
            Assert.Equal(expectedRequest, result);
            // Teardown
        }

        [Fact]
        public void DepthMatchesConstructorArgument()
        {
            // Fixture setup
            var dummyRequest = new object();
            var expectedDepth = 1;
            var sut = new RequestTraceEventArgs(dummyRequest, expectedDepth);
            // Exercise system
            int result = sut.Depth;
            // Verify outcome
            Assert.Equal(expectedDepth, result);
            // Teardown
        }
    }
}
