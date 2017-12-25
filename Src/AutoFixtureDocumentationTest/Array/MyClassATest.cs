using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Array
{
    public class MyClassATest
    {
        public MyClassATest()
        {
        }

        [Fact]
        public void CreatedSutCanHaveItemsAssignedSubsequently()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Create<MyClassA>();
            mc.Items = fixture.CreateMany<MyClassB>().ToArray();
            // Assert
            Assert.True(mc.Items.Length > 0);
            Assert.True(mc.Items.All(x => x != null));
        }

        [Fact]
        public void BuiltSutWillHavePopulatedItems()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Build<MyClassA>()
                .With(x => x.Items, 
                    fixture.CreateMany<MyClassB>().ToArray())
                .Create();
            // Assert
            Assert.True(mc.Items.Length > 0, "Non-empty array");
            Assert.True(mc.Items.All(x => x != null), "No item should be null");
        }

        [Fact]
        public void CustomizedSutWillHavePopulatedItems()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize<MyClassA>(ob =>
                ob.With(x => x.Items, 
                    fixture.CreateMany<MyClassB>().ToArray()));
            // Act
            var mc = fixture.Create<MyClassA>();
            // Assert
            Assert.True(mc.Items.Length > 0, "Non-empty array");
            Assert.True(mc.Items.All(x => x != null), "No item should be null");
        }

        [Fact]
        public void RegisterArrayWillProperlyPopulateItems()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Register<MyClassB[]>(() => 
                fixture.CreateMany<MyClassB>().ToArray());
            // Act
            var mc = fixture.Create<MyClassA>();
            // Assert
            Assert.True(mc.Items.Length > 0, "Non-empty array");
            Assert.True(mc.Items.All(x => x != null), "No item should be null");
        }
    }
}
