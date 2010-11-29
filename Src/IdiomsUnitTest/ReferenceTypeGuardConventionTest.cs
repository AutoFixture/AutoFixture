using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReferenceTypeGuardConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReferenceTypeGuardConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IValueGuardConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateInvalidsWithNullFixtureWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeGuardConvention>();
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
            var expected = new[] {
                typeof(NullReferenceBehavior)
            };

            var sut = fixture.CreateAnonymous<ReferenceTypeGuardConvention>();
            // Exercise system
            var result = (from invalid in sut.CreateInvalids(fixture) select invalid.GetType()).ToList();
            // Verify outcome
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }
    }
}
