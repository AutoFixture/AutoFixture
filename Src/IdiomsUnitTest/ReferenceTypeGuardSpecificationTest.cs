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
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(ReferenceTypeGuardSpecification));
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
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForValueType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ReferenceTypeGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(string));
            // Verify outcome
            Assert.IsType<ReferenceTypeGuardConvention>(result);
            // Teardown
        }

    }
}
