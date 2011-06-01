using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class WritablePropertyAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new FakeSpecimenBuilderComposer();
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
            var expectedComposer = new FakeSpecimenBuilderComposer();
            var sut = new WritablePropertyAssertion(expectedComposer);
            // Exercise system
            ISpecimenBuilderComposer result = sut.Composer;
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
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new FakeSpecimenBuilderComposer();
            var sut = new WritablePropertyAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }
    }
}
