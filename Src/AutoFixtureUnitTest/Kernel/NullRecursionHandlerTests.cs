using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NullRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullRecursionHandler();
            // Verify outcome
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
            // Teardown
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullRecursionHandler();
            // Exercise system
            var dummyRequest = new object();
            var dummyRequests = Enumerable.Empty<object>();
            var actual = sut.HandleRecursiveRequest(dummyRequest, dummyRequests);
            // Verify outcome
            Assert.Null(actual);
            // Teardown
        }
    }
}
