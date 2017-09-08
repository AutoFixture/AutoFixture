using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Array
{
    public class MyClassATest
    {
        public MyClassATest()
        {
        }

        [Fact]
        public void CreatedSutCanHaveItemsAssignedSubsequently()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Create<MyClassA>();
            mc.Items = fixture.CreateMany<MyClassB>().ToArray();
            // Verify outcome
            Assert.True(mc.Items.Length > 0);
            Assert.True(mc.Items.All(x => x != null));
            // Teardown
        }

        [Fact]
        public void BuiltSutWillHavePopulatedItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClassA>()
                .With(x => x.Items, 
                    fixture.CreateMany<MyClassB>().ToArray())
                .Create();
            // Verify outcome
            Assert.True(mc.Items.Length > 0, "Non-empty array");
            Assert.True(mc.Items.All(x => x != null), "No item should be null");
            // Teardown
        }

        [Fact]
        public void CustomizedSutWillHavePopulatedItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customize<MyClassA>(ob =>
                ob.With(x => x.Items, 
                    fixture.CreateMany<MyClassB>().ToArray()));
            // Exercise system
            var mc = fixture.Create<MyClassA>();
            // Verify outcome
            Assert.True(mc.Items.Length > 0, "Non-empty array");
            Assert.True(mc.Items.All(x => x != null), "No item should be null");
            // Teardown
        }

        [Fact]
        public void RegisterArrayWillProperlyPopulateItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Register<MyClassB[]>(() => 
                fixture.CreateMany<MyClassB>().ToArray());
            // Exercise system
            var mc = fixture.Create<MyClassA>();
            // Verify outcome
            Assert.True(mc.Items.Length > 0, "Non-empty array");
            Assert.True(mc.Items.All(x => x != null), "No item should be null");
            // Teardown
        }
    }
}
