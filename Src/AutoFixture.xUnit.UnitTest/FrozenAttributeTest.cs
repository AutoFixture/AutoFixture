using System;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit.UnitTest
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
        public void InitializeShouldSetDefaultMatchingStrategy()
        {
            // Arrange
            // Act
            var sut = new FrozenAttribute();
            // Assert
            Assert.Equal(Matching.ExactType, sut.By);
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
        public void GetCustomizationWithSpecificTypeShouldReturnCorrectResult()
        {
            // Arrange
            var registeredType = typeof(AbstractType);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = AParameter<ConcreteType>();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(registeredType, freezer.RegisteredType);
        }

        [Fact]
        [Obsolete]
        public void GetCustomizationWithIncompatibleSpecificTypeThrowsArgumentException()
        {
            // Arrange
            var registeredType = typeof(string);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = AParameter<ConcreteType>();
            // Act & assert
            Assert.Throws<ArgumentException>(() => sut.GetCustomization(parameter));
        }

        private static ParameterInfo AParameter<T>()
        {
            return typeof(SingleParameterType<T>)
                .GetConstructor(new[] { typeof(T) })
                .GetParameters()
                .Single(p => p.Name == "parameter");
        }
    }
}
