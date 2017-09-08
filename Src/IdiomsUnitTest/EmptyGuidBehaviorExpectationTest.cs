using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class EmptyGuidBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new EmptyGuidBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyNullCommandThrows()
        {
            // Fixture setup
            var sut = new EmptyGuidBehaviorExpectation();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(int))]
        public void VerifyDoesNothingWhenRequestedTypeIsNotGuid(Type type)
        {
            // Fixture setup
            var executeInvoked = false;
            var mockCommand = new DelegatingGuardClauseCommand { OnExecute = v => executeInvoked = true };
            mockCommand.RequestedType = type;

            var sut = new EmptyGuidBehaviorExpectation();
            // Exercise system
            sut.Verify(mockCommand);
            // Verify outcome
            Assert.False(executeInvoked);
            // Teardown
        }

        [Fact]
        public void VerifyCorrectlyInvokesExecuteWhenRequestedTypeIsGuid()
        {
            // Fixture setup
            var mockVerified = false;
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => mockVerified = Guid.Empty.Equals(v),
                OnCreateException = v => new InvalidOperationException(),
                RequestedType = typeof(Guid)
            };

            var sut = new EmptyGuidBehaviorExpectation();
            // Exercise system
            try
            {
                sut.Verify(mockCommand);
            }
            catch (InvalidOperationException) { }
            // Verify outcome
            Assert.True(mockVerified);
            // Teardown
        }

        [Fact]
        public void VerifySuccedsWhenCommandThrowsCorrectException()
        {
            // Fixture setup
            var cmd = new DelegatingGuardClauseCommand 
            {
                OnExecute = v => { throw new ArgumentException(); },
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(cmd));
            // Teardown
        }

        [Fact]
        public void VerifyThrowsWhenCommandThrowsUnexpectedException()
        {
            // Fixture setup
            var expectedInner = new Exception();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { throw expectedInner; },
                OnCreateExceptionWithInner = (v, e) => v == "\"Guid.Empty\"" && expectedInner.Equals(e) ? expected : new Exception(),
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();
            // Exercise system and verify outcome
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void VerifyThrowsWhenCommandDoesNotThrow()
        {
            // Fixture setup
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand
            {
                OnCreateException = v => v == "\"Guid.Empty\"" ? expected : new Exception(),
                RequestedType = typeof(Guid)
            };
            var sut = new EmptyGuidBehaviorExpectation();
            // Exercise system and verify outcome
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
