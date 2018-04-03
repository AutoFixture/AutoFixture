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
            var capturedValue = false;
            var createdException = new Exception();
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => capturedValue = string.Empty.Equals(v),
                OnCreateException = v => createdException,
                RequestedType = typeof(string)
            };

            var sut = new EmptyStringBehaviorExpectation();

            // Assert
            var ex = Assert.Throws<Exception>(() => 
                sut.Verify(mockCommand));

            Assert.Same(createdException, ex);
            Assert.True(capturedValue);
        }

        [Fact]
        public void VerifySucceedsWhenCommandThrowsCorrectException()
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { throw new ArgumentNullOrEmptyException(string.Empty); },
                RequestedType = typeof(string),
                RequestedParameterName = string.Empty
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
                OnCreateExceptionWithInner = (v, e) => v == "<empty string>" && expectedInner.Equals(e) ? expected : new Exception(),
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
                OnCreateException = v => v == "<empty string>" ? expected : new Exception(),
                RequestedType = typeof(string)
            };
            var sut = new EmptyStringBehaviorExpectation();

            // Act & Assert
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalidParamName")]
        public void VerifyThrowsWhenCommandThrowsArgumentNullExceptionWithInvalidParamName(string invalidParamName)
        {
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { throw new ArgumentNullOrEmptyException(invalidParamName); },
                RequestedType = typeof(string)
            };
            var sut = new EmptyStringBehaviorExpectation();
            Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
        }
    }
}