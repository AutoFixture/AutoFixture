using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using System.Reflection;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.Parsing
{
    [TestClass]
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [ExpectedException(typeof(TargetInvocationException))]
        [TestMethod]
        public void CreateWithDefaultStringWillThrow()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            fixture.CreateAnonymous<Contact>();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateWithExplicitNumberStringWillSucceed()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<string>(() => "112");
            Contact sut = fixture.CreateAnonymous<Contact>();
            // Exercise system
            int result = sut.PhoneNumber;
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), result, "PhoneNumber");
            // Teardown
        }

        [TestMethod]
        public void CreateWithAnonymousNumberStringWillSucceed()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<int, string>(i => i.ToString());
            Contact sut = fixture.CreateAnonymous<Contact>();
            // Exercise system
            int result = sut.PhoneNumber;
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), result, "PhoneNumber");
            // Teardown
        }
    }
}
