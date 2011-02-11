using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PropertyContextTest
    {
        [Fact]
        public void SutIsMemberContext()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var dummyProperty = typeof(string).GetProperties().First();
            // Exercise system
            var sut = new PropertyContext(dummyFixture, dummyProperty);
            // Verify outcome
            Assert.IsAssignableFrom<IMemberContext>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            var dummyProperty = typeof(string).GetProperties().First();
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                new PropertyContext(null, dummyProperty));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void InitializeWithNullPropertyInfoThrows()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                new PropertyContext(new Fixture(), null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var dummyProperty = typeof(string).GetProperties().First();
            var sut = new PropertyContext(expectedComposer, dummyProperty);
            // Exercise system
            ISpecimenBuilderComposer result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var expectedProperty = typeof(string).GetProperties().First();
            var sut = new PropertyContext(dummyFixture, expectedProperty);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(expectedProperty, result);
            // Teardown
        }

        [Fact]
        public void VerifyWritableWithReadOnlyPropertyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");

            var sut = new PropertyContext(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PropertyContextException), () =>
                sut.VerifyWritable());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableForIllBehavedPropertyGetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = typeof(IllBehavedPropertyHolder<object>).GetProperty("PropertyIllBehavedGet");

            var sut = new PropertyContext(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PropertyContextException), () =>
               sut.VerifyWritable());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableForIllBehavedPropertySetterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = typeof(IllBehavedPropertyHolder<object>).GetProperty("PropertyIllBehavedSet");

            var sut = new PropertyContext(fixture, propertyInfo);
            // Exercise system
            Assert.Throws(typeof(PropertyContextException), () =>
                sut.VerifyWritable());
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void VerifyWritableIsCorrectForWellBehavedProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");

            var sut = new PropertyContext(fixture, propertyInfo);
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
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");

            var sut = new PropertyContext(fixture, propertyInfo);
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

        [Fact]
        public void VerifyBoundariesOnReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var sut = new Fixture().ForProperty((SingleParameterType<object> x) => x.Parameter);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.VerifyBoundaries());
            // Teardown
        }
    }
}
