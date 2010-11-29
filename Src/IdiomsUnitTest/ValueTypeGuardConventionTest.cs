using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ValueTypeGuardConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new ValueTypeGuardConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IValueGuardConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsWithNullFixtureWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeGuardConvention>();
            // Exercise system
            var result = sut.CreateInvalids(fixture).Any();
            // Verify outcome
            Assert.False(result, "CreateInvalids");
            // Teardown
        }
    }
}
