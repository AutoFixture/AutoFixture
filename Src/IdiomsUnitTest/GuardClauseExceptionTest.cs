using System;
using System.Linq;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class GuardClauseExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Arrange
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            // Act
            var sut = new GuardClauseException();
            // Assert
            Assert.IsAssignableFrom<Exception>(sut);
        }

        [Fact]
        public void MessageIsNotNull()
        {
            // Arrange
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var sut = new GuardClauseException();
            // Act
            var result = sut.Message;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MessageIsCorrectWhenConstructedWithMessage()
        {
            // Arrange
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var expectedMessage = Guid.NewGuid().ToString();
            var sut = new GuardClauseException(expectedMessage);
            // Act
            var result = sut.Message;
            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void MessageIsCorrectWhenConstructedWithMessageAndInnerException()
        {
            // Arrange
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var expectedMessage = Guid.NewGuid().ToString();
            var dummyInner = new Exception();
            var sut = new GuardClauseException(expectedMessage, dummyInner);
            // Act
            var result = sut.Message;
            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void InnerExceptionIsCorrectWhenConstructedWithMessageAndInnerException()
        {
            // Arrange
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var dummyMessage = "Anonymous text";
            var expectedInner = new Exception();
            var sut = new GuardClauseException(dummyMessage, expectedInner);
            // Act
            var result = sut.InnerException;
            // Assert
            Assert.Equal(expectedInner, result);
        }
    }
}
