using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuardClauseExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            // Exercise system
            var sut = new GuardClauseException();
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void MessageIsNotNull()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var sut = new GuardClauseException();
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void MessageIsCorrectWhenConstructedWithMessage()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var expectedMessage = Guid.NewGuid().ToString();
            var sut = new GuardClauseException(expectedMessage);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal(expectedMessage, result);
            // Teardown
        }

        [Fact]
        public void MessageIsCorrectWhenConstructedWithMessageAndInnerException()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var expectedMessage = Guid.NewGuid().ToString();
            var dummyInner = new Exception();
            var sut = new GuardClauseException(expectedMessage, dummyInner);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal(expectedMessage, result);
            // Teardown
        }

        [Fact]
        public void InnerExceptionIsCorrectWhenConstructedWithMessageAndInnerException()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var dummyMessage = "Anonymous text";
            var expectedInner = new Exception();
            var sut = new GuardClauseException(dummyMessage, expectedInner);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal(expectedInner, result);
            // Teardown
        }
    }
}
