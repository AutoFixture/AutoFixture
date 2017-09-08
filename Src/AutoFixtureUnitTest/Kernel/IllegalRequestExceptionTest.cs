using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class IllegalRequestExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            // Exercise system
            var sut = new IllegalRequestException();
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void MessageWillBeDefineWhenDefaultConstructorIsUsed()
        {
            // Fixture setup
            var sut = new IllegalRequestException();
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void MessageWillMatchConstructorArgument()
        {
            // Fixture setup
            string expectedMessage = "Anonymous exception message";
            var sut = new IllegalRequestException(expectedMessage);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal(expectedMessage, result);
            // Teardown
        }

        [Fact]
        public void InnerExceptionWillMatchConstructorArgument()
        {
            // Fixture setup
            var expectedException = new ArgumentOutOfRangeException();
            var sut = new IllegalRequestException("Anonymous message.", expectedException);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal(expectedException, result);
            // Teardown
        }
    }
}
