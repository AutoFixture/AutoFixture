using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReferenceTypeBoundaryConventionTest
    {
        [Fact]
        public void SutIsIValueGuardConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReferenceTypeBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
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

            var sut = fixture.CreateAnonymous<ReferenceTypeBoundaryConvention>();
            // Exercise system
            var result = (from invalid in sut.CreateBoundaryBehaviors() select invalid.GetType()).ToList();
            // Verify outcome
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }
    }
}
