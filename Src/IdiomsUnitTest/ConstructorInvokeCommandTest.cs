using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ConstructorInvokeCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Fixture setup
            // Exercise system
            var sut = new ConstructorInvokeCommand();
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }
    }
}
