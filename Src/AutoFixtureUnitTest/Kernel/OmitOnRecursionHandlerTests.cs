using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitOnRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Fixture setup
            // Exercise system
            var sut = new OmitOnRecursionHandler();
            // Verify outcome
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
            // Teardown
        }
    }
}
