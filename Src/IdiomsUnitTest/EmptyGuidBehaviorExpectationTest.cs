using System;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EmptyGuidBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Arrange
            // Act
            var sut = new EmptyGuidBehaviorExpectation();
            // Assert
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
        }

        [Fact]
        public void VerifyNullCommandThrows()
        {
            // Arrange
            var sut = new EmptyGuidBehaviorExpectation();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify(null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(int))]
        public void VerifyDoesNothingWhenRequestedTypeIsNotGuid(Type type)
        {
            // Arrange
            var executeInvoked = false;
            var mockCommand = new DelegatingGuardClauseCommand { OnExecute = v => executeInvoked = true };
            mockCommand.RequestedType = type;

            var sut = new EmptyGuidBehaviorExpectation();
            // Act
            sut.Verify(mockCommand);
            // Assert
            Assert.False(executeInvoked);
        }

        [Fact]
        public void VerifyCorrectlyInvokesExecuteWhenRequestedTypeIsGuid()
        {
            // Arrange
            var mockVerified = false;
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => mockVerified = Guid.Empty.Equals(v),
                OnCreateException = v => new InvalidOperationException(),
                RequestedType = typeof(Guid)
            };

            var sut = new EmptyGuidBehaviorExpectation();
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
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();
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
                OnCreateExceptionWithInner = (v, e) => v == "\"Guid.Empty\"" && expectedInner.Equals(e) ? expected : new Exception(),
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();
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
                OnCreateException = v => v == "\"Guid.Empty\"" ? expected : new Exception(),
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();
            // Act & Assert
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerifyThrowsExpectedWhenCommandThrowsWithIncorrectParameterName()
        {
            // Arrange
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentException(string.Empty, "wrongParamName"),
                OnCreateExceptionWithFailureReason = (v, r, e) => expected,
                RequestedParameterName = "expectedParamName",
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => sut.Verify(cmd));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyThrowsWithExpectedMessageWhenCommandThrowsWithIncorrectParameterName()
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentException(string.Empty, "wrongParamName"),
                OnCreateExceptionWithFailureReason = (v, r, e) => new Exception(r, e),
                RequestedParameterName = "expectedParamName",
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => sut.Verify(cmd));

            // Assert
            Assert.EndsWith(
                string.Format("Expected parameter name: expectedParamName{0}Actual parameter name: wrongParamName",
                              Environment.NewLine),
                actual.Message);
        }

        [Fact]
        public void VerifyThrowsWithExpectedInnerWhenCommandThrowsWithIncorrectParameterName()
        {
            // Arrange
            var expected = new ArgumentException(string.Empty, "wrongParamName");
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw expected,
                OnCreateExceptionWithFailureReason = (v, r, e) => new Exception(r, e),
                RequestedParameterName = "expectedParamName",
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => sut.Verify(cmd));

            // Assert
            Assert.Equal(expected, actual.InnerException);
        }

        [Fact]
        public void VerifyDoesNotThrowWhenCommandThrowsWithCorrectParameterName()
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentException(string.Empty, "paramName"),
                OnCreateExceptionWithFailureReason = (v, r, e) => new Exception(r, e),
                RequestedParameterName = "paramName",
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => sut.Verify(cmd));

            // Assert
            Assert.Null(actual);
        }
    }
}
