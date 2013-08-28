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
    public class EqualsNullAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new EqualsNullAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new EqualsNullAssertion(expectedComposer);
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
                new EqualsNullAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
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
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals)));
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedEqualsNullOverrideDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(typeof(WellBehavedEqualsNullOverride)));
            // Teardown            
        }

        [Fact]
        public void VerifyIllbehavedEqualsNullBehaviourThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllbehavedEqualsNullOverride)));
            // Teardown
        }

        class IllbehavedEqualsNullOverride
        {
#pragma warning disable 659
            public override bool Equals(object obj)
#pragma warning restore 659
            {
                if (obj == null)
                {
                    return true;
                }
                throw new Exception();
            }
        }

        class WellBehavedEqualsNullOverride
        {
#pragma warning disable 659
            public override bool Equals(object obj)
#pragma warning restore 659
            {
                if (obj == null)
                {
                    return false;
                }
                throw new Exception();
            }
        }

        class ClassThatDoesNotOverrideObjectEquals
        {
        }
    }

}
