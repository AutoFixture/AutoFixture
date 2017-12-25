using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CurrentDateTimeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new CurrentDateTimeCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeWithNullThrowsArgumentNullException()
        {
            // Arrange
            var sut = new CurrentDateTimeCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsCurrentDateTimeGeneratorToFixture()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new CurrentDateTimeCustomization();
            // Act
            sut.Customize(fixture);
            // Assert
            Assert.True(fixture.Customizations.OfType<CurrentDateTimeGenerator>().Any());
        }
    }
}