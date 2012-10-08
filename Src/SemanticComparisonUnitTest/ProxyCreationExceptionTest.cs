﻿using System;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class ProxyCreationExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            // Exercise system
            var sut = new ProxyCreationException();
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void SutHasDefaultConstructor()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                new ProxyCreationException());
            // Teardown
        }

        [Fact]
        public void InitializedWithDefaultConstructorHasMessage()
        {
            // Fixture setup
            var sut = new ProxyCreationException();
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
            var sut = new ProxyCreationException(expectedMessage);
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
            var sut = new ProxyCreationException(expectedMessage, dummyException);
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
            const string dummeMessage = "Anonymous text";
            var expectedException = new Exception();
            var sut = new ProxyCreationException(dummeMessage, expectedException);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal(expectedException, result);
            // Teardown
        }
    }
}