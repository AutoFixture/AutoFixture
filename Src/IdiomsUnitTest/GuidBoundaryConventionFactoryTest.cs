using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuidBoundaryConventionFactoryTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuidBoundaryConventionFactory();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConventionFactory>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidBoundaryConventionFactory>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.GetConvention((Type)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForGuidType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof(Guid));
            // Verify outcome
            Assert.IsType<GuidBoundaryConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForNonGuidType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof (string));
            // Verify outcome
            Assert.IsAssignableFrom<NullBoundaryConvention>(result);
            // Teardown
        }
    }
}
