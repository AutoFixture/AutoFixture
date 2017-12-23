using System;
using System.Linq;
using AutoFixture;
using AutoFixture.DataAnnotations;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class NoDataAnnotationsCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Act
            var sut = new NoDataAnnotationsCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new NoDataAnnotationsCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Theory]
        [InlineData(typeof(RangeAttributeRelay))]
        [InlineData(typeof(StringLengthAttributeRelay))]
        [InlineData(typeof(RegularExpressionAttributeRelay))]
        [InlineData(typeof(NumericRangedRequestRelay))]
        [InlineData(typeof(EnumRangedRequestRelay))]
        public void CustomizeProperFixtureCorrectlyCustomizesIt(Type removedBuilderType)
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new NoDataAnnotationsCustomization();
            // Act
            sut.Customize(fixture);

            // Assert
            Assert.DoesNotContain(
                fixture.Customizations,
                b => b.GetType() == removedBuilderType);
        }
    }
}