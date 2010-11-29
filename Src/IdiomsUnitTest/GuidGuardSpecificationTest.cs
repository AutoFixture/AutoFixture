using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuidGuardSpecificationTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuidGuardSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<ITypeGuardSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidGuardSpecification>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.IsSatisfiedBy((Type)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForGuidType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(Guid));
            // Verify outcome
            Assert.IsType<GuidBoundaryConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForNonGuidType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<GuidGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof (string));
            // Verify outcome
            Assert.IsAssignableFrom<NullBoundaryConvention>(result);
            // Teardown
        }
    }
}
