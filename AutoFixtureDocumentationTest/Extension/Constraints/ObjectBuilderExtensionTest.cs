using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    [TestClass]
    public class ObjectBuilderExtensionTest
    {
        public ObjectBuilderExtensionTest()
        {
        }

        [TestMethod]
        public void CreateMyClassWithConstrainedPropertyWillCreateCorrectProperty()
        {
            // Fixture setup
            var minimum = 1;
            var maximum = 5;
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>()
                .With(x => x.SomeText, minimum, maximum)
                .CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(minimum <= mc.SomeText.Length && mc.SomeText.Length <= maximum, "SomeText within constraints.");
            // Teardown
        }

        [TestMethod]
        public void CreateMyClassWithPropertyConstrainedAsInTheFeatureRequest()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>()
                .With(x => x.SomeText, 0, 100)
                .CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(0 <= mc.SomeText.Length && mc.SomeText.Length <= 100, "SomeText within constraints.");
            // Teardown
        }
    }
}
