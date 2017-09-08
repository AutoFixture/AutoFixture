using System;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ObjectCreationExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            Type expectedBase = typeof(Exception);
            // Exercise system
            var sut = new ObjectCreationException();
            // Verify outcome
            Assert.IsAssignableFrom(expectedBase, sut);
            // Teardown
        }

        [Fact]
        public void MessageWillBeDefineWhenDefaultConstructorIsUsed()
        {
            // Fixture setup
            var sut = new ObjectCreationException();
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
            var sut = new ObjectCreationException(expectedMessage);
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
            var sut = new ObjectCreationException("Anonymous message.", expectedException);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal<Exception>(expectedException, result);
            // Teardown
        }
    }
}
