using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuidValueGuardConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuidValueGuardConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IValueGuardConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsWithNullFixtureWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidValueGuardConvention>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.CreateInvalids((Fixture)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void CreateInvalidsReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidValueGuardConvention>();
            // Exercise system
            var result = sut.CreateInvalids(fixture).Single();
            // Verify outcome
            Assert.IsType<GuidBoundaryBehavior>(result);
            // Teardown
        }
    }
}
