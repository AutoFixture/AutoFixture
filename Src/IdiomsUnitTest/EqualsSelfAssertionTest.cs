using System;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EqualsSelfAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new EqualsSelfAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new EqualsSelfAssertion(expectedComposer);
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
                new EqualsSelfAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsSelfAssertion(dummyComposer);
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
            var sut = new EqualsSelfAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals))));
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedEqualsSelfOverrideDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsSelfAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedEqualsSelfObjectOverride))));
            // Teardown            
        }

        [Fact]
        public void VerifyIllbehavedEqualsSelfBehaviourThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsSelfAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllBehavedEqualsSelfObjectOverride)));
            // Teardown
        }

#pragma warning disable 659
        class WellBehavedEqualsSelfObjectOverride
        {
            public override bool Equals(object obj)
            {
                if (obj != null && Object.ReferenceEquals(this, obj))
                    return true;

                throw new Exception();
            }
        }

        class IllBehavedEqualsSelfObjectOverride
        {
            public override bool Equals(object obj)
            {
                if (obj != null && Object.ReferenceEquals(this, obj))
                    return false;

                throw new Exception();
            }
        }
#pragma warning restore 659

        class ClassThatDoesNotOverrideObjectEquals
        {
        }
    }

}
