using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReferenceTypeBoundaryConventionFactoryTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReferenceTypeBoundaryConventionFactory();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConventionFactory>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeBoundaryConventionFactory>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.GetConvention((Type)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsNullForValueType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof(Guid));
            // Verify outcome
            Assert.IsAssignableFrom<NullBoundaryConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForReferenceType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof(string));
            // Verify outcome
            Assert.IsType<ReferenceTypeBoundaryConvention>(result);
            // Teardown
        }

    }
}
