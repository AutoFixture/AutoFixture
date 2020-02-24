using System;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class WhiteSpaceStringBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Arrange
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act & Assert
            Assert.IsAssignableFrom<IBehaviorExpectation>(expectation);
        }

        [Fact]
        public void VerifyNullCommandThrows()
        {
            // Arrange
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => expectation.Verify(default));
        }

        [Fact]
        public void VerifyExecutesCommandWhenRequestedTypeIsString()
        {
            // Arrange
            var commandExecuted = false;
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = (v) =>
                {
                    commandExecuted = true;
                    throw new ArgumentException(string.Empty, "paramName");
                },
                RequestedType = typeof(string),
                RequestedParameterName = "paramName"
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            expectation.Verify(command);

            // Assert
            Assert.True(commandExecuted);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(object))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(decimal))]
        public void VerifyDoesNotExecuteCommandWhenRequestedTypeNotString(Type type)
        {
            // Arrange
            var commandExecuted = false;
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = (v) => commandExecuted = true,
                RequestedType = type
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            expectation.Verify(command);

            // Assert
            Assert.False(commandExecuted);
        }

        [Fact]
        public void VerifyDoesNotThrowWhenCommandThrowsArgumentExceptionWithMatchingParameterName()
        {
            // Arrange
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentException(string.Empty, "paramName"),
                RequestedType = typeof(string),
                RequestedParameterName = "paramName"
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void VerifyThrowsExpectedExceptionWhenCommandDoesNotThrow()
        {
            // Arrange
            var expected = new Exception();
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { },
                OnCreateException = v => expected,
                RequestedType = typeof(string)
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyThrowsExceptionWithExpectedValueWhenCommandDoesNotThrow()
        {
            // Arrange
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { },
                OnCreateException = v => new Exception(v),
                RequestedType = typeof(string)
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal("<white space>", actual.Message);
        }

        [Fact]
        public void VerifyThrowsExpectedExceptionWhenCommandThrowsNonArgumentException()
        {
            // Arrange
            var expected = new Exception();
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new Exception(),
                OnCreateExceptionWithInner = (v, e) => expected,
                RequestedType = typeof(string)
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyThrowsWithExpectedValueWhenCommandThrowsNonArgumentException()
        {
            // Arrange
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new Exception(),
                OnCreateExceptionWithInner = (v, e) => new Exception(v, e),
                RequestedType = typeof(string)
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal("<white space>", actual.Message);
        }

        [Fact]
        public void VerifyThrowsWithExpectedInnerExceptionWhenCommandThrowsNonArgumentException()
        {
            // Arrange
            var expected = new Exception();
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw expected,
                OnCreateExceptionWithInner = (v, e) => new Exception(v, e),
                RequestedType = typeof(string)
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal(expected, actual.InnerException);
        }

        [Fact]
        public void VerifyThrowsExpectedExceptionWhenCommandThrowsArgumentExceptionWithWrongParameterName()
        {
            // Arrange
            var expected = new Exception();
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentException(string.Empty, "wrongParamName"),
                OnCreateExceptionWithFailureReason = (v, m, e) => expected,
                RequestedType = typeof(string),
                RequestedParameterName = "expectedParamName"
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyThrowsExceptionWithExpectedMessageWhenCommandThrowsArgumentExceptionWithWrongParameterName()
        {
            // Arrange
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentException(string.Empty, "wrongParamName"),
                OnCreateExceptionWithFailureReason = (v, m, e) => new Exception(m, e),
                RequestedType = typeof(string),
                RequestedParameterName = "expectedParamName"
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.EndsWith(
                $"Expected parameter name: expectedParamName{Environment.NewLine}Actual parameter name: wrongParamName",
                actual.Message);
        }

        [Fact]
        public void VerifyThrowsExceptionWithExpectedInnerWhenCommandThrowsArgumentExceptionWithWrongParameterName()
        {
            // Arrange
            var expected = new ArgumentException(string.Empty, "wrongParamName");
            var command = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw expected,
                OnCreateExceptionWithFailureReason = (v, m, e) => new Exception(m, e),
                RequestedType = typeof(string),
                RequestedParameterName = "expectedParamName"
            };
            var expectation = new WhiteSpaceStringBehaviorExpectation();

            // Act
            var actual = Record.Exception(() => expectation.Verify(command));

            // Assert
            Assert.Equal(expected, actual.InnerException);
        }
    }
}
