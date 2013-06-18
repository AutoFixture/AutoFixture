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
    }
}
