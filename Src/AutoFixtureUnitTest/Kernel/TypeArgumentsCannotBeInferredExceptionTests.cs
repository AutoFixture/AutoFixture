using System;
using System.Reflection;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TypeArgumentsCannotBeInferredExceptionTests
    {
        [Fact]
        public void SutIsException()
        {
            var sut = new TypeArgumentsCannotBeInferredException();
            Assert.IsAssignableFrom<Exception>(sut);
        }

        [Fact]
        public void MessageWillBeDefineWhenDefaultConstructorIsUsed()
        {
            var sut = new TypeArgumentsCannotBeInferredException();
            var result = sut.Message;
            Assert.NotNull(result);
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TypeArgumentsCannotBeInferredException((MethodInfo) null));
        }

        [Fact]
        public void MessageWillBeDefineWhenInitializedWithMethodInfo()
        {
            Action dummy = delegate { };
            var sut = new TypeArgumentsCannotBeInferredException(dummy.GetMethodInfo());

            var result = sut.Message;

            Assert.NotNull(result);
        }

        [Fact]
        public void MessageWillMatchConstructorArgument()
        {
            string expectedMessage = "Anonymous exception message";
            var sut = new TypeArgumentsCannotBeInferredException(expectedMessage);

            var result = sut.Message;
            
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void InnerExceptionWillMatchConstructorArgument()
        {
            var expectedException = new ArgumentOutOfRangeException();
            var sut = new TypeArgumentsCannotBeInferredException("Anonymous message.", expectedException);

            var result = sut.InnerException;
            
            Assert.Equal(expectedException, result);
        }
    }
}