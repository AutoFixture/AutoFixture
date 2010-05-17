using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class StringGuardSpecificationTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(StringGuardSpecification));
            // Verify outcome
            Assert.IsAssignableFrom<ITypeGuardSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringGuardSpecification>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.IsSatisfiedBy((Type) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWillReturnCorrectResultForString()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof (string));
            // Verify outcome
            Assert.IsType<StringGuardConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWillReturnsNullForNonString()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(Guid));
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

    }
}
