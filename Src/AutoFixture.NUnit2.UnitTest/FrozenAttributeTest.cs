using System;
using System.Linq;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit2.UnitTest
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

        [Test]
        [Obsolete]
        public void GetCustomizationWithSpecificRegisteredTypeReturnsCorrectResult()
        {
            // Arrange
            var registeredType = typeof(AbstractType);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            Assert.IsAssignableFrom<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.AreEqual(registeredType, freezer.RegisteredType);
        }

        [Test]
        [Obsolete]
        public void GetCustomizationWithIncompatibleRegisteredTypeThrowsArgumentException()
        {
            // Arrange
            var registeredType = typeof(string);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.GetCustomization(parameter));
        }
    }
}
