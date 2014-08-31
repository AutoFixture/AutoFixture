using System;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class TypeArgumentsCannotBeInferedExceptionTests
    {
        [Fact]
        public void SutIsException()
        {
            var sut = new TypeArgumentsCannotBeInferedException(string.Empty);
            Assert.IsAssignableFrom<Exception>(sut);
        }

        [Fact]
        public void MessageWillBeDefineWhenInitializedWithMethodInfo()
        {
            Action dummy = delegate { };
            var sut = new TypeArgumentsCannotBeInferedException(dummy.Method);

            var result = sut.Message;
            
            Assert.NotNull(result);
        }

        [Fact]
        public void MessageWillMatchConstructorArgument()
        {
            string expectedMessage = "Anonymous exception message";
            var sut = new TypeArgumentsCannotBeInferedException(expectedMessage);

            var result = sut.Message;
            
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void InnerExceptionWillMatchConstructorArgument()
        {
            var expectedException = new ArgumentOutOfRangeException();
            var sut = new TypeArgumentsCannotBeInferedException("Anonymous message.", expectedException);

            var result = sut.InnerException;
            
            Assert.Equal(expectedException, result);
        }
    }
}