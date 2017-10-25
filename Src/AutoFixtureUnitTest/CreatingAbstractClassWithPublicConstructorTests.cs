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
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create<AbstractClassWithPublicConstructor>());
        }

        [Fact]
        public void MapAbstractClassWithPublicConstructorToTestDoubleToWorkAroundException()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(AbstractClassWithPublicConstructor),
                    typeof(TestDouble)));
            // Exercise system
            var actual = fixture.Create<AbstractClassWithPublicConstructor>();
            // Verify outcome
            Assert.IsAssignableFrom<TestDouble>(actual);
            // Teardown
        }

        private class TestDouble : AbstractClassWithPublicConstructor
        {
        }
    }
}
