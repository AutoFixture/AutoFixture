using System;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EqualsNewObjectAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(expectedComposer);
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
                new EqualsNewObjectAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
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
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals))));
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedEqualsNullOverrideDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedEqualsNewObjectOverride))));
            // Teardown            
        }

        [Fact]
        public void VerifyIllbehavedEqualsNullBehaviourThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllBehavedEqualsNewObjectOverride)));
            // Teardown
        }

#pragma warning disable 659
        class WellBehavedEqualsNewObjectOverride
        {
            public override bool Equals(object obj)
            {
                if (obj != null && obj.GetType() == typeof(object))
                    return false;

                throw new Exception();
            }
        }

        class IllBehavedEqualsNewObjectOverride
        {
            public override bool Equals(object obj)
            {
                if (obj != null && obj.GetType() == typeof(object))
                    return true;

                throw new Exception();
            }
        }
#pragma warning restore 659

        class ClassThatDoesNotOverrideObjectEquals
        {
        }
    }

}
