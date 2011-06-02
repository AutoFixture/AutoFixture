using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PropertySetCommandTest
    {
        [Fact]
        public void SutIsContextualAction()
        {
            // Fixture setup
            // Exercise system
            var sut = new PropertySetCommand();
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }
    }
}
