using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class NullBoundaryConventionTest
    {
        [Fact]
        public void SutIsValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullBoundaryConvention();
            // Exercise system
            Fixture dummyFixture = null;
            var result = sut.CreateBoundaryBehaviors();
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }
    }
}
