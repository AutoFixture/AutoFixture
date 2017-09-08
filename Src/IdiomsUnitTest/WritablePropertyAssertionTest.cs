using System;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class WritablePropertyAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new WritablePropertyAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new WritablePropertyAssertion(expectedComposer);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new WritablePropertyAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new WritablePropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyIllBehavedPropertyGetterThrows()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new WritablePropertyAssertion(composer);
            
            var propertyInfo = typeof(IllBehavedPropertyHolder<object>).GetProperty("PropertyIllBehavedGet");
            // Exercise system and verify outcome
            var e = Assert.Throws<WritablePropertyException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.PropertyInfo);
            // Teardown
        }

        [Fact]
        public void VerifyIllBehavedPropertySetterThrows()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new WritablePropertyAssertion(composer);

            var propertyInfo = typeof(IllBehavedPropertyHolder<object>).GetProperty("PropertyIllBehavedSet");
            // Exercise system and verify outcome
            var e = Assert.Throws<WritablePropertyException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.PropertyInfo);
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new WritablePropertyAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedWritablePropertyDoesNotThrow()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new WritablePropertyAssertion(composer);

            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }
    }
}
