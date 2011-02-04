using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuidBoundaryConventionTest
    {
        [Fact]
        public void SutIsBoundaryConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuidBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsWithNullTypeThrows()
        {
            // Fixture setup
            var sut = new GuidBoundaryConvention();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateBoundaryBehaviors(null).ToList());
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsForGuidReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new GuidBoundaryConvention();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(typeof(Guid));
            // Verify outcome
            Assert.True(result.OfType<GuidBoundaryBehavior>().Any());
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsForNonGuidReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new GuidBoundaryConvention();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(typeof(string));
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
