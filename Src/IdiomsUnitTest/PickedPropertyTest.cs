using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
#warning Revisit this when ConstructorArgumentMatches<>() and AssertConstructorInvariantsOf<>() have been implemented
    public class PickedPropertyTest
    {
        [Fact]
        public void SutIsIPickedProperty()
        {
            // Fixture setup
            // Exercise system
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PickedProperty<object, object>));
            // Verify outcome
            Assert.IsAssignableFrom<IPickedProperty>(sut);
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
        public void AssertInvariantsWithNullWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<PropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<PropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.AssertInvariants((IEnumerable<ITypeGuardSpecification>) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertInvariantsWithListThatCannotHandlePropertyTypeWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<PropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);

            var sut = new PickedProperty<PropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(ArgumentException), () =>
                sut.AssertInvariants(Enumerable.Empty<ITypeGuardSpecification>()));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertInvariantsWillAssertCorrectly()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = Reflect<InvariantReferenceTypePropertyHolder<object>>.GetProperty(propertyHolder => propertyHolder.Property);
            var typeSpec = new MyTypeGuardSpecification();

            var sut = new PickedProperty<InvariantReferenceTypePropertyHolder<object>, object>(fixture, propertyInfo);
            // Exercise system
            sut.AssertInvariants(Enumerable.Repeat(typeSpec, 1).Cast<ITypeGuardSpecification>());
            // Verify outcome
            Assert.NotNull(typeSpec.valueGuardConvention.testInvalidValue.AssertAction);
            // Teardown
        }

        private class MyTypeGuardSpecification : ITypeGuardSpecification
        {
            public readonly MyValueGuardConvention valueGuardConvention;

            public MyTypeGuardSpecification()
            {
                this.valueGuardConvention = new MyValueGuardConvention();
            }

            #region Implementation of ITypeGuardSpecification

            public IValueGuardConvention IsSatisfiedBy(Type type)
            {
                return this.valueGuardConvention;
            }

            #endregion
        }

        private class MyValueGuardConvention : IValueGuardConvention
        {
            public readonly TestBoundaryBehavior testInvalidValue;

            public MyValueGuardConvention()
            {
                this.testInvalidValue = new TestBoundaryBehavior(); ;
            }

            #region Implementation of IValueGuardConvention

            public IEnumerable<IBoundaryBehavior> CreateInvalids(Fixture fixture)
            {
                return Enumerable.Repeat(this.testInvalidValue, 1).Cast<IBoundaryBehavior>();
            }

            #endregion
        }

        private class TestBoundaryBehavior : IBoundaryBehavior
        {
            private Action<object> assertAction;
            public Action<object> AssertAction { get { return this.assertAction; } }

            #region Implementation of IBoundaryBehavior

            public void Assert(Action<object> action)
            {
                this.assertAction = action;
                action(null);
            }

            public bool IsSatisfiedBy(Type exceptionType)
            {
                return true;
            }

            public string Description
            {
                get { return string.Empty; }
            }

            #endregion
        }

    }
}
