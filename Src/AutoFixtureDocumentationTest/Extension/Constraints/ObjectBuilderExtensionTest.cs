using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Extension.Constraints
{
    public class ObjectBuilderExtensionTest
    {
        public ObjectBuilderExtensionTest()
        {
        }

        [Fact]
        public void CreateMyClassWithConstrainedPropertyWillCreateCorrectProperty()
        {
            // Arrange
            var minimum = 1;
            var maximum = 5;
            var fixture = new Fixture();
            // Act
            var mc = fixture.Build<MyClass>()
                .With(x => x.SomeText, minimum, maximum)
                .Create();
            // Assert
            Assert.True(minimum <= mc.SomeText.Length && mc.SomeText.Length <= maximum, "SomeText within constraints.");
        }

        [Fact]
        public void CreateMyClassWithPropertyConstrainedAsInTheFeatureRequest()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Build<MyClass>()
                .With(x => x.SomeText, 0, 100)
                .Create();
            // Assert
            Assert.True(0 <= mc.SomeText.Length && mc.SomeText.Length <= 100, "SomeText within constraints.");
        }
    }
}
