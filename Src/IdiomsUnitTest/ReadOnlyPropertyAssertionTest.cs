using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReadOnlyPropertyAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(expectedComposer);
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
                new ReadOnlyPropertyAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyNullConstructorInfoThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((ConstructorInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            var constructorWithNoParameters = typeof (PropertyHolder<object>).GetConstructors().First();
            Assert.Equal(0, constructorWithNoParameters.GetParameters().Length);
            Assert.DoesNotThrow(() =>
                sut.Verify(constructorWithNoParameters));
            // Teardown
        }

        [Fact]
        public void VerifyWritablePropertyDoesNotThrow()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(composer);

            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithNoMatchingConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            var e = Assert.Throws<ReadOnlyPropertyException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.PropertyInfo);
            // Teardown
        }
        
        [Fact]
        public void VerifyReadOnlyPropertyGetsValuePassedToConstructor()
        {
            // Given a PropertyInfo or FieldInfo, it should verify that for all 
            // constructor having a matching argument, the value should be preserved
            // Fixture setup
            // Exercise system and verify outcome
            // Teardown
            Assert.True(false);
        }

        [Fact]
        public void VerifyReadOnlyFieldGetsValuePassedToConstructor()
        {
            // Given a PropertyInfo or FieldInfo, it should verify that for all 
            // constructor having a matching argument, the value should be preserved
            // Fixture setup
            // Exercise system and verify outcome
            // Teardown
            Assert.True(false);
        }

        [Fact]
        public void VerifyConstructorArgumentsAreExposedAsFields()
        {
            // Given a ConstructorInfo, it should verify that all constructor arguments 
            // are properly exposed as either fields or Inspection Properties.
            // Fixture setup
            // Exercise system and verify outcome
            // Teardown
            Assert.True(false);
        }

        [Fact]
        public void VerifyConstructorArgumentsAreExposedAsProperties()
        {
            // Given a ConstructorInfo, it should verify that all constructor arguments 
            // are properly exposed as either fields or Inspection Properties.
            // Fixture setup
            // Exercise system and verify outcome
            // Teardown
            Assert.True(false);
        }
    }
}
