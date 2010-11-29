using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class NullValueGuardConventionTest
    {
        [Fact]
        public void SutIsValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullValueGuardConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IValueGuardConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullValueGuardConvention();
            // Exercise system
            Fixture dummyFixture = null;
            var result = sut.CreateBoundaryBehaviors(dummyFixture);
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }
    }
}
