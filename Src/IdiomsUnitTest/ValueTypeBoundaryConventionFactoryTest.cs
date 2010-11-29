using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ValueTypeBoundaryConventionFactoryTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new ValueTypeBoundaryConventionFactory();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConventionFactory>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeBoundaryConventionFactory>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.GetConvention((Type) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForReferenceType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof(string));
            // Verify outcome
            Assert.IsAssignableFrom<NullBoundaryConvention>(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForValueType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeBoundaryConventionFactory>();
            // Exercise system
            var result = sut.GetConvention(typeof(DateTime));
            // Verify outcome
            Assert.IsType<ValueTypeBoundaryConvention>(result);
            // Teardown
        }
    }
}
