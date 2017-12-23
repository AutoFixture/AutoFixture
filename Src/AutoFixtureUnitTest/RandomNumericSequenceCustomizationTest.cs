using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomNumericSequenceCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new RandomNumericSequenceCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeWithNullFixtureThrows()
        {
            // Arrange
            var sut = new RandomNumericSequenceCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsCorrectBuilderToFixture()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new RandomNumericSequenceCustomization();
            // Act
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<RandomNumericSequenceGenerator>()
                .SingleOrDefault();
            // Assert
            Assert.NotNull(result);
        }
    }
}