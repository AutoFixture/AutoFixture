using System;
using NUnit.Framework;

namespace AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class FrozenAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new FrozenAttribute();
            // Assert
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new FrozenAttribute();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }
    }
}
