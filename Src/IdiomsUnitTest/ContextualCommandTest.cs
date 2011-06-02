using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ContextualCommandTest
    {
        [Fact]
        public void SutIsContextualAction()
        {
            // Fixture setup
            // Exercise system
            var sut = new ContextualCommand();
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }
    }
}
