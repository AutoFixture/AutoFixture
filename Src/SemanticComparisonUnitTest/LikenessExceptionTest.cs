using System;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class LikenessExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            // Exercise system
            var sut = new LikenessException();
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void SutHasDefaultConstructor()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                new LikenessException()
            ));
            // Teardown
        }

        [Fact]
        public void InitializedWithDefaultConstructorHasMessage()
        {
            // Fixture setup
            var sut = new LikenessException();
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result));
            // Teardown
        }

        [Fact]
        public void InitializedWithMessageHasCorrectMessage()
        {
            // Fixture setup
            var expectedMessage = Guid.NewGuid().ToString();
            var sut = new LikenessException(expectedMessage);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal(expectedMessage, result);
            // Teardown
        }

        [Fact]
        public void InitializedWithMessageAndInnerExceptionHasCorrectMessage()
        {
            // Fixture setup
            var expectedMessage = Guid.NewGuid().ToString();
            var dummyException = new Exception();
            var sut = new LikenessException(expectedMessage, dummyException);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal(expectedMessage, result);
            // Teardown
        }

        [Fact]
        public void InitializedWithMessageAndInnerExceptionHasInnerException()
        {
            // Fixture setup
            var dummyMessage = "Anonymous text";
            var expectedException = new Exception();
            var sut = new LikenessException(dummyMessage, expectedException);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal(expectedException, result);
            // Teardown
        }
    }
}
