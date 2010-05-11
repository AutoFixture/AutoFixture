using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    [TestClass]
    public class PickedPropertyExceptionTest
    {
        [TestMethod]
        public void SutIsException()
        {
            // Fixture setup
            Type expectedType = typeof(Exception);
            // Exercise system
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PickedPropertyException));
            // Verify outcome
            Assert.IsInstanceOfType(sut, expectedType);
            // Teardown
        }

        [TestMethod]
        public void MessageIsNotNull()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = new PickedPropertyException();
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.IsNotNull(result, "Message");
            // Teardown
        }

        [TestMethod]
        public void MessageIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = fixture.CreateAnonymous("Message");

            var sut = new PickedPropertyException(expected);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.AreEqual<string>(expected, result, "Message");
            // Teardown
        }

        [TestMethod]
        public void InnerExceptionIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = fixture.CreateAnonymous<Exception>();

            var sut = new PickedPropertyException(fixture.CreateAnonymous("Message"), expected);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.AreEqual<Exception>(expected, result, "InnerException");
            // Teardown
        }
    }
}