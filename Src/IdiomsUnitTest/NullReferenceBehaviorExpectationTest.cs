using System;
using AutoFixture.Idioms;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class NullReferenceBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Arrange
            // Act
            var sut = new NullReferenceBehaviorExpectation();
            // Assert
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
        }

        [Fact]
        public void VerifyNullCommandThrows()
        {
            // Arrange
            var sut = new NullReferenceBehaviorExpectation();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify(null));
        }

        [Theory]
        [InlineData(typeof(TriState))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTime))]
        public void VerifyDoesNothingWhenContextTypeIsNotInterfaceOrReference(Type type)
        {
            // Arrange
            var verifyInvoked = false;
            var mockCommand = new DelegatingGuardClauseCommand { OnExecute = v => verifyInvoked = true };
            mockCommand.RequestedType = type;

            var sut = new NullReferenceBehaviorExpectation();
            // Act
            sut.Verify(mockCommand);
            // Assert
            Assert.False(verifyInvoked, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyCorrectlyInvokesExecuteForNullableContexts(Type type)
        {
            // Arrange
            var mockVerified = false;
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => mockVerified = v == null,
                OnCreateException = v => new InvalidOperationException()
            };
            mockCommand.RequestedType = type;

            var sut = new NullReferenceBehaviorExpectation();
            // Act
            try
            {
                sut.Verify(mockCommand);
            }
            catch (InvalidOperationException) { }
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void VerifySucceedsWhenCommandThrowsCorrectException()
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => { throw new ArgumentNullException(); } };
            var sut = new NullReferenceBehaviorExpectation();
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(cmd)));
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalidParamName")]
        public void VerifyThrowsWhenCommandThrowsArgumentNullExceptionWithInvalidParamName(string invalidParamName)
        {
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentNullException(invalidParamName)
            };
            var sut = new NullReferenceBehaviorExpectation();
            Assert.Throws<Exception>(() => 
                sut.Verify(cmd));
        }

        [Fact]
        public void VerifyThrowsWithCorrectMessageWhenCommandThrowsArgumentNullExceptionWithInvalidParamName()
        {
            // Arrange
            var invalidParamName = "invalidParameterName";
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw new ArgumentNullException(invalidParamName),
                OnCreateExceptionWithFailureReason = (v, r, ie) => new Exception(r, ie)
            };
            var sut = new NullReferenceBehaviorExpectation();
            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Contains(invalidParamName, ex.Message);
            Assert.Contains("Guard Clause", ex.Message);
        }

        [Fact]
        public void VerifyThrowsWhenCommandThrowsUnexpectedException()
        {
            // Arrange
            var expectedInner = new Exception();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => throw expectedInner,
                OnCreateExceptionWithInner = (v, e) => v == "null" && expectedInner.Equals(e) ? expected : new Exception()
            };
            var sut = new NullReferenceBehaviorExpectation();
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
                OnCreateException = v => v == "null" ? expected : new Exception()
            };
            var sut = new NullReferenceBehaviorExpectation();
            // Act & Assert
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
        }
    }
}
