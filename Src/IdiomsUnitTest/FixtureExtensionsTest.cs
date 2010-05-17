using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class FixtureExtensionsTest
    {
        [Fact]
        public void PickPropertyWithNullExpressionWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                new Fixture().PickProperty<object, object>(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void PickPropertyWithNullFuncWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentException), () =>
                new Fixture().PickProperty<object, object>(sut => (object)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void PickPropertyCanPickReadOnlyProperty()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().PickProperty((ReadOnlyPropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void PickPropertyWithReadWritePropertyWillReturnInstance()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().PickProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void PickPropertyWithReadWritePropertyWillReturnCorrectType()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().PickProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.IsType<PickedProperty<PropertyHolder<object>, object>>(result);
            // Teardown
        }

        [Fact]
        public void PickPropertyReturnsUsableInstance()
        {
            // Fixture setup
            // Exercise system
            new Fixture()
                .PickProperty((PropertyHolder<object> ph) => ph.Property)
                .IsWellBehavedWritableProperty();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void PickPropertyCanAssertInvariants()
        {
            // Fixture setup
            new Fixture()
                .PickProperty((InvariantReferenceTypePropertyHolder<object> iph) => iph.Property)
                .AssertInvariants();
            // Exercise system
            // Verify outcome (no exception indicates success)
            // Teardown
        }
    }
}
