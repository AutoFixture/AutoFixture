using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValueObject
{
    [TestClass]
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [TestMethod]
        public void CreateWillNotThrow()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            fixture.CreateAnonymous<Contact>();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [TestMethod]
        public void CreateWillPopulatePhoneNumber()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            Contact sut = fixture.CreateAnonymous<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), result, "PhoneNumber.RawNumber");
            // Teardown
        }
    }
}
