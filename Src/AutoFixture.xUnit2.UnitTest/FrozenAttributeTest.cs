using System;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest
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

        [Fact]
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
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(registeredType, freezer.RegisteredType);
        }

        [Fact]
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
            // Act & assert
            Assert.Throws<ArgumentException>(() => sut.GetCustomization(parameter));
        }
    }
}
