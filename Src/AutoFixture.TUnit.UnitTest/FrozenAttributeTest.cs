using System;

namespace AutoFixture.TUnit.UnitTest
{
    public class FrozenAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new FrozenAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Test]
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
