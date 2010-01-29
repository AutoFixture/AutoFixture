using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValidatingValueObject
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
            fixture.Register<int>(() => 12345678);
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
            fixture.Register<int, DanishPhoneNumber>(i => 
                new DanishPhoneNumber(i + 112));
            Contact sut = fixture.CreateAnonymous<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), result, "PhoneNumber.RawNumber");
            // Teardown
        }

        [TestMethod]
        public void UsingPhoneNumberMinValue()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<int, DanishPhoneNumber>(i =>
                new DanishPhoneNumber(i + 
                    DanishPhoneNumber.MinValue));
            Contact sut = fixture.CreateAnonymous<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), result, "PhoneNumber.RawNumber");
            // Teardown
        }
    }
}
