using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class EqualsSuccessiveAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(expectedComposer);
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
                new EqualsSuccessiveAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectEqualsDoesNothing()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals)));
            // Teardown
        }

        class ClassThatDoesNotOverrideObjectEquals
        {
        }
    }

}
