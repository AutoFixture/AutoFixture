using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class RequestTraceEventArgsTest
    {
        [Fact]
        public void SutIsEventArgs()
        {
            // Arrange
            var dummyRequest = new object();
            var dummyDepth = 0;
            // Act
            var sut = new RequestTraceEventArgs(dummyRequest, dummyDepth);
            // Assert
            Assert.IsAssignableFrom<EventArgs>(sut);
        }

        [Fact]
        public void ConstructorExplicitlyAllowsTracingNullRequestsIfThatShouldEverBeWarranted()
        {
            // Arrange
            var dummyDepth = 0;
            // Act & assert
            Assert.Null(Record.Exception(() => new RequestTraceEventArgs(null, dummyDepth)));
        }

        [Fact]
        public void RequestMatchesConstructorArgument()
        {
            // Arrange
            var expectedRequest = new object();
            var dummyDepth = 0;
            var sut = new RequestTraceEventArgs(expectedRequest, dummyDepth);
            // Act
            var result = sut.Request;
            // Assert
            Assert.Equal(expectedRequest, result);
        }

        [Fact]
        public void DepthMatchesConstructorArgument()
        {
            // Arrange
            var dummyRequest = new object();
            var expectedDepth = 1;
            var sut = new RequestTraceEventArgs(dummyRequest, expectedDepth);
            // Act
            int result = sut.Depth;
            // Assert
            Assert.Equal(expectedDepth, result);
        }
    }
}
