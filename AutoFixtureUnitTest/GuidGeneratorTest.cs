using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class GuidGeneratorTest
    {
        public GuidGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillReturnNonDefaultGuid()
        {
            // Fixture setup
            var unexpectedGuid = default(Guid);
            // Exercise system
            var result = GuidGenerator.CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<Guid>(unexpectedGuid, result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousTwiceWillReturnDifferentValues()
        {
            // Fixture setup
            var unexpectedGuid = GuidGenerator.CreateAnonymous();
            // Exercise system
            var result = GuidGenerator.CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<Guid>(unexpectedGuid, result, "CreateAnonymous");
            // Teardown
        }
    }
}
