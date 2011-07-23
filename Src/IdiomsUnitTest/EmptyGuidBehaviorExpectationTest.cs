using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class EmptyGuidBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new EmptyGuidBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }
    }
}
