using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using System.Text.RegularExpressions;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class GuidStringGeneratorTest
    {
        public GuidStringGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateWithNullNameWillReturnGuidString()
        {
            // Fixture setup
            Guid unexpectedGuid = Guid.Empty;
            string nullName = null;
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(nullName);
            // Verify outcome
            Guid g = new Guid(result);
            Assert.AreNotEqual<Guid>(unexpectedGuid, g, "Created string");
            // Teardown
        }

        [TestMethod]
        public void CreateWithEmptyNameWillReturnGuidString()
        {
            // Fixture setup
            Guid unexpectedGuid = Guid.Empty;
            string emptyName = string.Empty;
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(emptyName);
            // Verify outcome
            Guid g = new Guid(result);
            Assert.AreNotEqual<Guid>(unexpectedGuid, g, "Created string");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNameWillReturnStringThatBeginsWithName()
        {
            // Fixture setup
            string expectedName = "AnonymousName";
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(expectedName);
            // Verify outcome
            string name = new TextGuidRegex().GetText(result);
            Assert.AreEqual<string>(expectedName, name, "Name part");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNameWillReturnStringThatEndsInGuid()
        {
            // Fixture setup
            Guid unexpectedGuid = Guid.Empty;
            string anonymousName = "AnonymousName";
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(anonymousName);
            // Verify outcome
            string guidString = new TextGuidRegex().GetGuid(result);
            Guid g = new Guid(guidString);
            Assert.AreNotEqual<Guid>(unexpectedGuid, g, "Guid part");
            // Teardown
        }
    }
}
