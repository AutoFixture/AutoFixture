using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class StringBoundaryConventionFactoryTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringBoundaryConventionFactory();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConventionFactory>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringBoundaryConventionFactory>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.GetConvention((Type) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWillReturnCorrectResultForString()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof (string));
            // Verify outcome
            Assert.IsType<StringBoundaryConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWillReturnCorrectResultForNonString()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<StringBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof(Guid));
            // Verify outcome
            Assert.IsAssignableFrom<NullBoundaryConvention>(result);
            // Teardown
        }

    }
}
