using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    [TestClass]
    public class FixtureExtensionsTest
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void PickPropertyWithNullExpressionWillThrow()
        {
            // Fixture setup
            // Exercise system
            new Fixture().PickProperty<object, object>(null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void PickPropertyWithNullFuncWillThrow()
        {
            // Fixture setup
            // Exercise system
            new Fixture().PickProperty<object, object>(sut => (object)null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void PickPropertyCanPickReadOnlyProperty()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().PickProperty((ReadOnlyPropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.IsNotNull(result, "PickProperty");
            // Teardown
        }

        [TestMethod]
        public void PickPropertyWithReadWritePropertyWillReturnInstance()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().PickProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.IsNotNull(result, "PickProperty");
            // Teardown
        }

        [TestMethod]
        public void PickPropertyWithReadWritePropertyWillReturnCorrectType()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().PickProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(PickedProperty<PropertyHolder<object>, object>));
            // Teardown
        }

        [TestMethod]
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
    }
}
