using System;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class GetHashCodeSuccessiveAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(expectedComposer);
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
                new GetHashCodeSuccessiveAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectGetHashCodeDoesNothing()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectGetHashCode))));
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedGetHashCodeSelfOverrideDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedGetHashCodeSelfObjectOverride))));
            // Teardown            
        }

        [Fact]
        public void VerifyIllbehavedEqualsSelfBehaviourThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<GetHashCodeOverrideException>(() =>
                sut.Verify(typeof(IllBehavedEqualsSelfObjectOverride)));
            // Teardown
        }

#pragma warning disable 659
        class WellBehavedGetHashCodeSelfObjectOverride
        {
            public override int GetHashCode()
            {
                return 666;
            }
        }

        class IllBehavedEqualsSelfObjectOverride
        {
            private static readonly Random HashCodeGenerator = new Random();

            public override int GetHashCode()
            {
                return HashCodeGenerator.Next();
            }
        }
#pragma warning restore 659

        class ClassThatDoesNotOverrideObjectGetHashCode
        {
        }
    }
}