using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest
{
    public class FavorArraysAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new FavorArraysAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new FavorArraysAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new FavorArraysAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), new[] { typeof(object) })!
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<ArrayFavoringConstructorQuery>(invoker.Query);
        }
    }
}
