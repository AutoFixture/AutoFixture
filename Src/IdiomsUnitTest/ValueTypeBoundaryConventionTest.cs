using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ValueTypeBoundaryConventionTest
    {
        [Fact]
        public void SutIsBoundaryConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new ValueTypeBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ValueTypeBoundaryConvention();
            var dummyType = typeof(object);
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(dummyType);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
