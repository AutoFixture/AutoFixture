using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReferenceTypeGuardSpecificationTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReferenceTypeGuardSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<ITypeGuardSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeGuardSpecification>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.IsSatisfiedBy((Type)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsNullForValueType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(Guid));
            // Verify outcome
            Assert.IsAssignableFrom<NullBoundaryConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForReferenceType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(string));
            // Verify outcome
            Assert.IsType<ReferenceTypeBoundaryConvention>(result);
            // Teardown
        }

    }
}
