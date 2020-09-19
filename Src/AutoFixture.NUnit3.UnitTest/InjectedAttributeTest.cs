using NUnit.Framework;

namespace AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class InjectedAttributeTest
    {
        [Test, AutoData]
        public void SutIsAttribute(int value)
        {
            // Arrange
            // Act
            var sut = new InjectedAttribute(value);
            // Assert
            Assert.That(sut, Is.InstanceOf<CustomizeAttribute>());
        }

        [Test, AutoData]
        public void GetCustomizationFromNullParameterThrows(int value)
        {
            // Arrange
            var sut = new InjectedAttribute(value);
            // Act & Assert
            Assert.That(() => sut.GetCustomization(null), Throws.ArgumentNullException);
        }

        [Test, AutoData]
        public void ValueToInjectReturnsConstructorParameter(int value)
        {
            // Arrange
            var sut = new InjectedAttribute(value);
            // Act & Assert
            Assert.That(sut.ValueToInject, Is.EqualTo(value));
        }
    }
}