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
        public void SutIsBoundaryConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullBoundaryConvention();
            var dummyType = typeof(object);
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(dummyType);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
