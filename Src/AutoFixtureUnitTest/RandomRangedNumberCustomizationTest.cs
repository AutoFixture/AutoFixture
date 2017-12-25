using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    [Obsolete]
    public class RandomRangedNumberCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new RandomRangedNumberCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeWithNullFixtureThrows()
        {
            // Arrange
            var sut = new RandomRangedNumberCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsCorrectBuilderToFixture()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new RandomRangedNumberCustomization();
            // Act
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<RandomRangedNumberGenerator>()
                .SingleOrDefault();
            // Assert
            Assert.NotNull(result);
        }
    }
}