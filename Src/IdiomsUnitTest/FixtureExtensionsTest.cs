using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class FixtureExtensionsTest
    {
        [Fact]
        public void ForPropertyWithNullExpressionWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                new Fixture().ForProperty<object, object>(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ForPropertyWithNullFuncWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentException), () =>
                new Fixture().ForProperty<object, object>(sut => (object)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ForPropertyCanPickReadOnlyProperty()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().ForProperty((ReadOnlyPropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ForPropertyWithReadWritePropertyWillReturnInstance()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().ForProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ForPropertyWithReadWritePropertyWillReturnCorrectType()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().ForProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.IsType<PropertyContext<PropertyHolder<object>, object>>(result);
            // Teardown
        }

        [Fact]
        public void ForPropertyReturnsUsableInstance()
        {
            // Fixture setup
            // Exercise system
            new Fixture()
                .ForProperty((PropertyHolder<object> ph) => ph.Property)
                .VerifyWritable();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void ForPropertyCanAssertInvariants()
        {
            // Fixture setup
            new Fixture()
                .ForProperty((InvariantReferenceTypePropertyHolder<object> iph) => iph.Property)
                .VerifyBoundaries();
            // Exercise system
            // Verify outcome (no exception indicates success)
            // Teardown
        }
    }
}
