using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    public class ObjectBuilderExtensionTest
    {
        public ObjectBuilderExtensionTest()
        {
        }

        [Fact]
        public void CreateMyClassWithConstrainedPropertyWillCreateCorrectProperty()
        {
            // Fixture setup
            var minimum = 1;
            var maximum = 5;
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>()
                .With(x => x.SomeText, minimum, maximum)
                .Create();
            // Verify outcome
            Assert.True(minimum <= mc.SomeText.Length && mc.SomeText.Length <= maximum, "SomeText within constraints.");
            // Teardown
        }

        [Fact]
        public void CreateMyClassWithPropertyConstrainedAsInTheFeatureRequest()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>()
                .With(x => x.SomeText, 0, 100)
                .Create();
            // Verify outcome
            Assert.True(0 <= mc.SomeText.Length && mc.SomeText.Length <= 100, "SomeText within constraints.");
            // Teardown
        }
    }
}
