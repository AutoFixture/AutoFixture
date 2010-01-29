using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ObjectCreationExceptionTest
    {
        public ObjectCreationExceptionTest()
        {
        }

        [TestMethod]
        public void SutIsException()
        {
            // Fixture setup
            Type expectedBase = typeof(Exception);
            // Exercise system
            var sut = new ObjectCreationException();
            // Verify outcome
            Assert.IsInstanceOfType(sut, expectedBase);
            // Teardown
        }

        [TestMethod]
        public void MessageWillBeDefineWhenDefaultConstructorIsUsed()
        {
            // Fixture setup
            var sut = new ObjectCreationException();
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.IsNotNull(result, "Message");
            // Teardown
        }

        [TestMethod]
        public void MessageWillMatchConstructorArgument()
        {
            // Fixture setup
            string expectedMessage = "Anonymous exception message";
            var sut = new ObjectCreationException(expectedMessage);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.AreEqual<string>(expectedMessage, result, "Message");
            // Teardown
        }

        [TestMethod]
        public void InnerExceptionWillMatchConstructorArgument()
        {
            // Fixture setup
            var expectedException = new ArgumentOutOfRangeException();
            var sut = new ObjectCreationException("Anonymous message.", expectedException);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.AreEqual<Exception>(expectedException, result, "InnerException");
            // Teardown
        }
    }
}
