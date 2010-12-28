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
                new PickedProperty<object, object>((Fixture)null, Reflect<string>.GetProperty(s => s.Length)));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void CreateWithNullPropertyInfoWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                new PickedProperty<object, object>(new Fixture(), (PropertyInfo)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableWithReadOnlyPropertyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<ReadOnlyPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<ReadOnlyPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PickedPropertyException), () =>
                sut.VerifyWritable());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableForIllBehavedPropertyGetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<IllBehavedPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.PropertyIllBehavedGet);

            var sut = new PickedProperty<IllBehavedPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PickedPropertyException), () =>
               sut.VerifyWritable());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableForIllBehavedPropertySetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<IllBehavedPropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.PropertyIllBehavedSet);

            var sut = new PickedProperty<IllBehavedPropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PickedPropertyException), () =>
                sut.VerifyWritable());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableIsCorrectForWellBehavedProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<PropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<PropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            sut.VerifyWritable();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesWithNullWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<PropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<PropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                sut.VerifyBoundaries(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesCorrectlyAssertsBehaviors()
        {
            // Fixture setup
            var invocations = 0;
            var behaviors = new[]
            {
                new DelegatingBoundaryBehavior{ OnAssert = a => invocations++ },
                new DelegatingBoundaryBehavior{ OnAssert = a => invocations++ },
                new DelegatingBoundaryBehavior{ OnAssert = a => invocations++ }
            };

            var convention = new DelegatingBoundaryConvention { OnCreateBoundaryBehaviors = t => t == typeof(object) ? behaviors : Enumerable.Empty<IBoundaryBehavior>() };

            var sut = new Fixture().ForProperty((PropertyHolder<object> ph) => ph.Property);
            // Exercise system
            sut.VerifyBoundaries(convention);
            // Verify outcome
            Assert.Equal(behaviors.Length, invocations);
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesThrowsWhenSutHasIncorrectBoundaryBehavior()
        {
            // Fixture setup
            var sut = new Fixture().ForProperty((PropertyHolder<object> ph) => ph.Property);
            // Exercise system and verify outcome
            Assert.Throws<BoundaryConventionException>(() =>
                sut.VerifyBoundaries());
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesDoesNotThrowWhenSutHasCorrectBoundaryBehavior()
        {
            // Fixture setup
            var sut = new Fixture().ForProperty((InvariantReferenceTypePropertyHolder<object> x) => x.Property);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.VerifyBoundaries());
            // Teardown
        }
    }
}
