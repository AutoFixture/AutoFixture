using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CreatingAbstractClassWithPublicConstructorTests
    {
        [Fact]
        public void CreateAbstractWithPublicConstructorWillThrow()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create<AbstractClassWithPublicConstructor>());
        }

        [Fact]
        public void MapAbstractClassWithPublicConstructorToTestDoubleToWorkAroundException()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(AbstractClassWithPublicConstructor),
                    typeof(TestDouble)));
            // Act
            var actual = fixture.Create<AbstractClassWithPublicConstructor>();
            // Assert
            Assert.IsAssignableFrom<TestDouble>(actual);
        }

        private class TestDouble : AbstractClassWithPublicConstructor
        {
        }
    }
}
