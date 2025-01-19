using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest
{
    public class ModestAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Arrange & Act
            var sut = new ModestAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new ModestAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
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
