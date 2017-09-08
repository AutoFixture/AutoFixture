using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class NullReferenceBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullReferenceBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyNullCommandThrows()
        {
            // Fixture setup
            var sut = new NullReferenceBehaviorExpectation();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TriState))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTime))]
        public void VerifyDoesNothingWhenContextTypeIsNotInterfaceOrReference(Type type)
        {
            // Fixture setup
            var verifyInvoked = false;
            var mockCommand = new DelegatingGuardClauseCommand { OnExecute = v => verifyInvoked = true };
            mockCommand.RequestedType = type;

            var sut = new NullReferenceBehaviorExpectation();
            // Exercise system
            sut.Verify(mockCommand);
            // Verify outcome
            Assert.False(verifyInvoked, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyCorrectlyInvokesExecuteForNullableContexts(Type type)
        {
            // Fixture setup
            var mockVerified = false;
            var mockCommand = new DelegatingGuardClauseCommand
            {
                OnExecute = v => mockVerified = v == null,
                OnCreateException = v => new InvalidOperationException()
            };
            mockCommand.RequestedType = type;

            var sut = new NullReferenceBehaviorExpectation();
            // Exercise system
            try
            {
                sut.Verify(mockCommand);
            }
            catch (InvalidOperationException) { }
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void VerifySucceedsWhenCommandThrowsCorrectException()
        {
            // Fixture setup
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => { throw new ArgumentNullException(); } };
            var sut = new NullReferenceBehaviorExpectation();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(cmd));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalidParamName")]
        public void VerifyThrowsWhenCommandThrowsArgumentNullExceptionWithInvalidParamName(string invalidParamName)
        {
            var cmd = new DelegatingGuardClauseCommand
            {
                OnExecute = v => { throw new ArgumentNullException(invalidParamName); }
            };
            var sut = new NullReferenceBehaviorExpectation();
            Assert.Throws<Exception>(() => 
                sut.Verify(cmd));
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
                OnCreateExceptionWithInner = (v, e) => v == "null" && expectedInner.Equals(e) ? expected : new Exception()
            };
            var sut = new NullReferenceBehaviorExpectation();
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
                OnCreateException = v => v == "null" ? expected : new Exception()
            };
            var sut = new NullReferenceBehaviorExpectation();
            // Exercise system and verify outcome
            var result = Assert.Throws<Exception>(() =>
                sut.Verify(cmd));
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
