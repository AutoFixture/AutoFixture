using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuidBoundaryConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuidBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsWithNullFixtureWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidBoundaryConvention>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.CreateBoundaryBehaviors((Fixture)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void CreateInvalidsReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidBoundaryConvention>();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(fixture).Single();
            // Verify outcome
            Assert.IsType<GuidBoundaryBehavior>(result);
            // Teardown
        }
    }
}
