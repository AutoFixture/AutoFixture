using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    [TestClass]
    public class PersonTest
    {
        public PersonTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillThrow()
        {
            var fixture = new Fixture();
            // var person = fixture.CreateAnonymous<Person>();

            /* The above call to CreateAnonymous will throw a StackOverflowException, which cannot
             * be caught in .NET 2+.
             * To stop the whole unit test suite from crashing, the line has been commented out. To
             * reproduce this behavior, uncomment the line that creates a new anonymous Person. */
        }

        [TestMethod]
        public void BuildWithoutSpouseWillSucceed()
        {
            var fixture = new Fixture();
            var person = fixture.Build<Person>()
                .Without(p => p.Spouse)
                .CreateAnonymous();

            Assert.IsNotNull(person, "Anonymous person");
        }

        [TestMethod]
        public void SettingSpouseIsPossible()
        {
            // Fixture setup
            var fixture = new Fixture();
            var person = fixture.Build<Person>().Without(p => p.Spouse).CreateAnonymous();
            var sut = fixture.Build<Person>().Without(p => p.Spouse).CreateAnonymous();
            // Exercise system
            sut.Spouse = person;
            // Verify outcome
            Assert.AreEqual<Person>(sut, person.Spouse, "Spouse");
            // Teardown
        }
    }
}
