using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }
    }
}
