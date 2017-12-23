using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public abstract class FixtureResolvingItselfTests<T>
    {
        [Fact]
        public void FixtureCanResolveItself()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var actual = sut.Create<T>();
            // Assert
            Assert.Equal<object>(sut, actual);
        }
    }

    public class FixtureResolvingItselfTestsOfFixture : FixtureResolvingItselfTests<Fixture> { }
    public class FixtureResolvingItselfTestsOfFixtureInterface : FixtureResolvingItselfTests<IFixture> { }
    public class FixtureResolvingItselfTestsOfSpecimenBuilder : FixtureResolvingItselfTests<ISpecimenBuilder> { }
}
