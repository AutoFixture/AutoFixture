using System;
using NUnit.Framework;

namespace AutoFixture.NUnit4.UnitTest
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
            Assert.That(sut, Is.InstanceOf<CustomizeAttribute>());
        }

        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.That(sut, Is.InstanceOf<Attribute>());
        }

        [Test]
        public void SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.That(sut, Is.InstanceOf<IParameterCustomizationSource>());
        }
    }
}
