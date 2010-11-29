using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class StringBoundaryConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsWithNullFixtureWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringBoundaryConvention>();
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
            var expected = new[] {
                typeof(EmptyStringBehavior)
            };

            var sut = fixture.CreateAnonymous<StringBoundaryConvention>();
            // Exercise system
            var result = (from invalid in sut.CreateBoundaryBehaviors(fixture) select invalid.GetType()).ToList();
            // Verify outcome
            Assert.True(expected.SequenceEqual(result), "CreateInvalids");
            // Teardown
        }
    }
}
