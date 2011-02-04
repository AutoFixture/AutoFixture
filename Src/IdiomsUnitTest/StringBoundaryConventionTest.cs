using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class StringBoundaryConventionTest
    {
        [Fact]
        public void SutIsBoundaryConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsWithNullTypeThrows()
        {
            // Fixture setup
            var sut = new StringBoundaryConvention();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateBoundaryBehaviors(null).ToList());
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsForStringReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new StringBoundaryConvention();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(typeof(string));
            // Verify outcome
            Assert.True(result.OfType<EmptyStringBehavior>().Any());
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsForNonStringReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new StringBoundaryConvention();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(typeof(Guid));
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
