using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            ClassicAssert.IsInstanceOf<Attribute>(sut);
        }

        [Test]
        public void SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            ClassicAssert.IsInstanceOf<IParameterCustomizationSource>(sut);
        }
    }
}
