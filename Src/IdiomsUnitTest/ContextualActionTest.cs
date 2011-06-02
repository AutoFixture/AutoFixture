using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ContextualActionTest
    {
        [Fact]
        public void SutIsContextualAction()
        {
            // Fixture setup
            // Exercise system
            var sut = new ContextualAction();
            // Verify outcome
            Assert.IsAssignableFrom<IContextualAction>(sut);
            // Teardown
        }
    }
}
