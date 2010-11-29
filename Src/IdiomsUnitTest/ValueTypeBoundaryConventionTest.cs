using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ValueTypeBoundaryConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new ValueTypeBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsWithNullFixtureWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeBoundaryConvention>();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(fixture).Any();
            // Verify outcome
            Assert.False(result, "CreateInvalids");
            // Teardown
        }
    }
}
