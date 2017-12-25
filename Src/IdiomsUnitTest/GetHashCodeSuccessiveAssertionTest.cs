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
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(expectedComposer);
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
                new GetHashCodeSuccessiveAssertion(null));
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectGetHashCodeDoesNothing()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectGetHashCode))));
        }

        [Fact]
        public void VerifyWellBehavedGetHashCodeSelfOverrideDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedGetHashCodeSelfObjectOverride))));
        }

        [Fact]
        public void VerifyIllbehavedEqualsSelfBehaviourThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new GetHashCodeSuccessiveAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<GetHashCodeOverrideException>(() =>
                sut.Verify(typeof(IllBehavedEqualsSelfObjectOverride)));
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