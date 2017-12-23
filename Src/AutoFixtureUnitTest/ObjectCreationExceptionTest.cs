using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class ObjectCreationExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Arrange
            Type expectedBase = typeof(Exception);
            // Act
            var sut = new ObjectCreationException();
            // Assert
            Assert.IsAssignableFrom(expectedBase, sut);
        }

        [Fact]
        public void MessageWillBeDefineWhenDefaultConstructorIsUsed()
        {
            // Arrange
            var sut = new ObjectCreationException();
            // Act
            var result = sut.Message;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MessageWillMatchConstructorArgument()
        {
            // Arrange
            string expectedMessage = "Anonymous exception message";
            var sut = new ObjectCreationException(expectedMessage);
            // Act
            var result = sut.Message;
            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void InnerExceptionWillMatchConstructorArgument()
        {
            // Arrange
            var expectedException = new ArgumentOutOfRangeException();
            var sut = new ObjectCreationException("Anonymous message.", expectedException);
            // Act
            var result = sut.InnerException;
            // Assert
            Assert.Equal<Exception>(expectedException, result);
        }
    }
}
