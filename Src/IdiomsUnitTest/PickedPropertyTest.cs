using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PickedPropertyTest
    {
        [Fact]
        public void SutIsIPickedProperty()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var dummyProperty = Reflect<string>.GetProperty(s => s.Length);
            // Exercise system
            var sut = new PickedProperty<object, object>(dummyFixture, dummyProperty);
            // Verify outcome
            Assert.IsAssignableFrom<IPickedProperty>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsVerifiableBoundary()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var dummyProperty = Reflect<string>.GetProperty(s => s.Length);
            // Exercise system
            var sut = new PickedProperty<object, object>(dummyFixture, dummyProperty);
            // Verify outcome
            Assert.IsAssignableFrom<IVerifiableBoundary>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullFixtureWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                new PickedProperty<object, object>((Fixture) null, Reflect<string>.GetProperty(s => s.Length)));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void CreateWithNullPropertyInfoWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                new PickedProperty<object, object>(new Fixture(), (PropertyInfo) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsWellBehavedWritablePropertyWithReadOnlyPropertyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<ReadOnlyPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<ReadOnlyPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PickedPropertyException), () =>
                sut.IsWellBehavedWritableProperty());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsWellBehavedWritablePropertyForIllBehavedPropertyGetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<IllBehavedPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.PropertyIllBehavedGet);

            var sut = new PickedProperty<IllBehavedPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PickedPropertyException), () =>
               sut.IsWellBehavedWritableProperty());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsWellBehavedWritablePropertyForIllBehavedPropertySetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<IllBehavedPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.PropertyIllBehavedSet);

            var sut = new PickedProperty<IllBehavedPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PickedPropertyException), () =>
                sut.IsWellBehavedWritableProperty());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
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

        [Fact]
        public void VerifyBoundaryBehaviorWithNullWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<PropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<PropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                sut.VerifyBoundaryBehavior(null));
            // Verify outcome (expected exception)
            // Teardown
        }
    }
}
