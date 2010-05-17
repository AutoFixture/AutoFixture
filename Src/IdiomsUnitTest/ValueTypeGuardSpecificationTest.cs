using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ValueTypeGuardSpecificationTest
    {
        [Fact]
        public void SutIsITypeGuardSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(ValueTypeGuardSpecification));
            // Verify outcome
            Assert.IsAssignableFrom<ITypeGuardSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeGuardSpecification>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.IsSatisfiedBy((Type) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsNullForReferenceType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(string));
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResultForValueType()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<ValueTypeGuardSpecification>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(DateTime));
            // Verify outcome
            Assert.IsType<ValueTypeGuardConvention>(result);
            // Teardown
        }
    }
}
