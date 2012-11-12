using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class CompositeNodeComposerTests
    {
        [Fact]
        public void SutIsComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeNodeComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }
    }
}
