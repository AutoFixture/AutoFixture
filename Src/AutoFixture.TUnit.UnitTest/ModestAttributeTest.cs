using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest
{
    public class ModestAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange & Act
            var sut = new ModestAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new ModestAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new ModestAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
            .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), new[] { typeof(object) })!
            .GetParameters().Single();

            // Act
            var result = sut.GetCustomization(parameter);

            // Assert
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<ModestConstructorQuery>(invoker.Query);
        }
    }
}
