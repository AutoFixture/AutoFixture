using System;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class CustomizeAttributeTest
    {
        [Test]
        public void TestableSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsInstanceOf<Attribute>(sut);
        }

        [Test]
        public void SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsInstanceOf<IParameterCustomizationSource>(sut);
        }
    }
}
