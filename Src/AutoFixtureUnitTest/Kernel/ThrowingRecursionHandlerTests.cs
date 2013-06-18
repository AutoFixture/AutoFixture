using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ThrowingRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Fixture setup
            // Exercise system
            var sut = new ThrowingRecursionHandler();
            // Verify outcome
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
            // Teardown
        }

        [Fact]
        public void HandleRecursiveRequestThrows()
        {
            // Fixture setup
            var sut = new ThrowingRecursionHandler();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(
                () => sut.HandleRecursiveRequest(
                    new object(),
                    new[] { new object() }));
            // Teardown
        }
    }
}
