using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using System.Text.RegularExpressions;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class GuidStringGeneratorTest
    {
        public GuidStringGeneratorTest()
        {
        }

        [Fact]
        public void CreateWithNullNameWillReturnGuidString()
        {
            // Fixture setup
            Guid unexpectedGuid = Guid.Empty;
            string nullName = null;
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(nullName);
            // Verify outcome
            Guid g = new Guid(result);
            Assert.NotEqual<Guid>(unexpectedGuid, g);
            // Teardown
        }

        [Fact]
        public void CreateWithEmptyNameWillReturnGuidString()
        {
            // Fixture setup
            Guid unexpectedGuid = Guid.Empty;
            string emptyName = string.Empty;
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(emptyName);
            // Verify outcome
            Guid g = new Guid(result);
            Assert.NotEqual<Guid>(unexpectedGuid, g);
            // Teardown
        }

        [Fact]
        public void CreateWithNameWillReturnStringThatBeginsWithName()
        {
            // Fixture setup
            string expectedName = "AnonymousName";
            // Exercise system
            string result = GuidStringGenerator.CreateAnonymous(expectedName);
            // Verify outcome
            string name = new TextGuidRegex().GetText(result);
            Assert.Equal<string>(expectedName, name);
            // Teardown
        }

        [Fact]
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
            Assert.NotEqual<Guid>(unexpectedGuid, g);
            // Teardown
        }
    }
}
