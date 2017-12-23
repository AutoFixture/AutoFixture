using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomBooleanSequenceCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Act
            var sut = new RandomBooleanSequenceCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new RandomBooleanSequenceCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeProperFixtureCorrectlyCustomizesIt()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new RandomBooleanSequenceCustomization();
            // Act
            sut.Customize(fixture);
            var result = fixture.Customizations.OfType<RandomBooleanSequenceGenerator>().SingleOrDefault();
            // Assert
            Assert.NotNull(result);
        }


    }
}