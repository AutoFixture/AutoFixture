using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReflectionExceptionUnwrappingCommandTest
    {
        [Fact]
        public void SutIsContextualCommand()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReflectionExceptionUnwrappingCommand();
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }
    }
}
