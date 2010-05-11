using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
#warning Revisit this when ConstructorArgumentMatches<>() and AssertConstructorInvariantsOf<>() have been implemented
    [TestClass]
    public class PickedPropertyTest
    {
        [TestMethod]
        public void SutIsIPickedProperty()
        {
            // Fixture setup
            Type expectedType = typeof(IPickedProperty);
            // Exercise system
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PickedProperty<object,object>));
            // Verify outcome
            Assert.IsInstanceOfType(sut, expectedType);
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullFixtureWillThrow()
        {
            // Fixture setup
            // Exercise system
            new PickedProperty<object, object>((Fixture) null, Reflect<string>.GetProperty(s => s.Length));
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullPropertyInfoWillThrow()
        {
            // Fixture setup
            // Exercise system
            new PickedProperty<object, object>(new Fixture(), (PropertyInfo)null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void IsWellBehavedWritablePropertyWithReadOnlyPropertyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<ReadOnlyPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<ReadOnlyPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            sut.IsWellBehavedWritableProperty();
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void IsWellBehavedWritablePropertyForIllBehavedPropertyGetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<IllBehavedPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.PropertyIllBehavedGet);

            var sut = new PickedProperty<IllBehavedPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            sut.IsWellBehavedWritableProperty();
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void IsWellBehavedWritablePropertyForIllBehavedPropertySetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<IllBehavedPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.PropertyIllBehavedSet);

            var sut = new PickedProperty<IllBehavedPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            sut.IsWellBehavedWritableProperty();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void IsWellBehavedWritablePropertyIsCorrectForWellBehavedProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<PropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<PropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            sut.IsWellBehavedWritableProperty();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

    }
}
