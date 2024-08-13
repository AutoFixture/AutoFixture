using System;
using Xunit;

namespace AutoFixture.Xunit.v3.UnitTest
{
    public class FrozenAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new FrozenAttribute();

            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new FrozenAttribute();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                                                     sut.GetCustomization(null));
        }
    }
}