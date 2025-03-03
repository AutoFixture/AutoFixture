using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
#if NETCOREAPP1_1
using System.Reflection;
#endif

namespace AutoFixture.TUnit.UnitTest
{
    public class GreedyAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new GreedyAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new GreedyAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new GreedyAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), new[] { typeof(object) })!
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<GreedyConstructorQuery>(invoker.Query);
        }
    }
}
