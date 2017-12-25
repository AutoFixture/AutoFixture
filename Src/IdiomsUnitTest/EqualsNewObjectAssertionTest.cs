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
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(expectedComposer);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EqualsNewObjectAssertion(null));
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectEqualsDoesNothing()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals))));
        }

        [Fact]
        public void VerifyWellBehavedEqualsNullOverrideDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedEqualsNewObjectOverride))));
        }

        [Fact]
        public void VerifyIllbehavedEqualsNullBehaviourThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNewObjectAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllBehavedEqualsNewObjectOverride)));
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
