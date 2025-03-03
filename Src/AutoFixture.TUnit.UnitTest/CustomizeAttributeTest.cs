using System;
using AutoFixture.TUnit.UnitTest.TestTypes;

namespace AutoFixture.TUnit.UnitTest
{
    public class CustomizeAttributeTest
    {
        [Test]
        public void TestableSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<Attribute>(sut);
        }

        [Test]
        public void SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<IParameterCustomizationSource>(sut);
        }
    }
}
