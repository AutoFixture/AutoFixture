using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class IncrementingDateTimeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new IncrementingDateTimeCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeWithNullThrowsArgumentNullException()
        {
            // Arrange
            var sut = new IncrementingDateTimeCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsIncrementingDateTimeGeneratorToTheFixture()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new IncrementingDateTimeCustomization();
            // Act
            sut.Customize(fixture);
            // Assert
            var containsIncrementingDateTimeGenerator = fixture
                .Customizations
                .OfType<StrictlyMonotonicallyIncreasingDateTimeGenerator>()
                .Any();
            Assert.True(containsIncrementingDateTimeGenerator);
        }
    }
}
