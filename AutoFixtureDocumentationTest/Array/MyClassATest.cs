using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Array
{
    [TestClass]
    public class MyClassATest
    {
        public MyClassATest()
        {
        }

        [TestMethod]
        public void CreatedSutCanHaveItemsAssignedSubsequently()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.CreateAnonymous<MyClassA>();
            mc.Items = fixture.CreateMany<MyClassB>().ToArray();
            // Verify outcome
            Assert.IsTrue(mc.Items.Length > 0, "Non-empty array");
            Assert.IsTrue(mc.Items.All(x => x != null), "No item should be null");
            // Teardown
        }

        [TestMethod]
        public void BuiltSutWillHavePopulatedItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClassA>()
                .With(x => x.Items, 
                    fixture.CreateMany<MyClassB>().ToArray())
                .CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(mc.Items.Length > 0, "Non-empty array");
            Assert.IsTrue(mc.Items.All(x => x != null), "No item should be null");
            // Teardown
        }

        [TestMethod]
        public void CustomizedSutWillHavePopulatedItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customize<MyClassA>(ob =>
                ob.With(x => x.Items, 
                    fixture.CreateMany<MyClassB>().ToArray()));
            // Exercise system
            var mc = fixture.CreateAnonymous<MyClassA>();
            // Verify outcome
            Assert.IsTrue(mc.Items.Length > 0, "Non-empty array");
            Assert.IsTrue(mc.Items.All(x => x != null), "No item should be null");
            // Teardown
        }
    }
}
