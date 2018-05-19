using System;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EqualsSuccessiveAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(expectedComposer);
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
                new EqualsSuccessiveAssertion(null));
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectEqualsDoesNothing()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals))));
        }

        [Fact]
        public void VerifyWellBehavedEqualsSuccessiveOverrideDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedEqualsSuccessiveObjectOverride))));
        }

        [Fact]
        public void VerifyIllbehavedEqualsSuccessiveBehaviourThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllBehavedEqualsSuccessiveObjectOverride)));
        }

#pragma warning disable 659
        class WellBehavedEqualsSuccessiveObjectOverride
        {
            public override bool Equals(object obj)
            {
                return true;
            }
        }

        class IllBehavedEqualsSuccessiveObjectOverride
        {
            public int equalsCallCount;

            public override bool Equals(object obj)
            {
                return (++this.equalsCallCount % 2 == 0);
            }
        }
#pragma warning restore 659

        class ClassThatDoesNotOverrideObjectEquals
        {
        }
    }

}
