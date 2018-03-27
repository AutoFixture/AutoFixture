using System;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EmptyStringBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            //Arrange
            //Act
            var sut = new EmptyStringBehaviorExpectation();

            //Assert
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
        }

        [Fact]
        public void VerifyNullCommandThrows()
        {
            // Arrange
            var sut = new EmptyStringBehaviorExpectation();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify(null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(int))]
        public void VerifyDoesNothingWhenRequestedTypeIsNotString(Type type)
        {
            // Arrange
            var executeInvoked = false;
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => executeInvoked = true,
                RequestedType = type
            };

            var sut = new EmptyStringBehaviorExpectation();

            // Act
            sut.Verify(mockCommand);

            // Assert
            Assert.False(executeInvoked);
        }

        [Fact]
        public void VerifyCorrectlyInvokesExecuteWhenRequestedTypeIsString()
        {
            // Arrange
            var mockVerified = false;
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => mockVerified = string.Empty.Equals(v),
                OnCreateException = v => new InvalidOperationException(),
                RequestedType = typeof(string)
            };

            var sut = new EmptyStringBehaviorExpectation();

            // Act
            try
            {
                sut.Verify(mockCommand);
            }
            catch (InvalidOperationException) { }
            // Assert
            Assert.True(mockVerified);
        }

        [Fact]
        public void VerifySuccedsWhenCommandThrowsCorrectException()
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { throw new ArgumentException(); },
                RequestedType = typeof(string)
            };

            var sut = new EmptyStringBehaviorExpectation();

            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(cmd)));
        }

        [Fact]
        public void VerifyThrowsWhenCommandThrowsUnexpectedException()
        {
            // Arrange
            var expectedInner = new Exception();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { throw expectedInner; },
                OnCreateExceptionWithInner = (v, e) => v == "\"string.Empty\"" && expectedInner.Equals(e) ? expected : new Exception(),
                RequestedType = typeof(string)
            };
            var sut = new EmptyStringBehaviorExpectation();

            // Act & Assert
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerifyThrowsWhenCommandDoesNotThrow()
        {
            // Arrange
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand
            {
                OnCreateException = v => v == "\"string.Empty\"" ? expected : new Exception(),
                RequestedType = typeof(string)
            };
            var sut = new EmptyStringBehaviorExpectation();

            // Act & Assert
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
        }
    }
}