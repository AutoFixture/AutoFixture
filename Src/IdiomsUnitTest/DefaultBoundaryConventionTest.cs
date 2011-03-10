using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DefaultBoundaryConventionTest
    {
        [Fact]
        public void SutIsConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<CompositeBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void ConventionTypesAreCorrect()
        {
            // Fixture setup
            var expected = new[]{
                typeof(GuidBoundaryConvention),
                typeof(ReferenceTypeBoundaryConvention)
            };

            var sut = new DefaultBoundaryConvention();
            // Exercise system
            // Verify outcome
            var result = (from c in sut.Conventions
                          select c.GetType()).ToList();
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }
    }
}
