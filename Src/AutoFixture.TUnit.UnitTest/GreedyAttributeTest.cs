using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using System.Threading.Tasks;
#if NETCOREAPP1_1
using System.Reflection;
#endif

namespace AutoFixture.TUnit.UnitTest
{
    public class GreedyAttributeTest
    {
        [Test]
        public async Task SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new GreedyAttribute();
            // Assert
            await Assert.That(sut).IsAssignableFrom<CustomizeAttribute>();
        }

        [Test]
        public async Task GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new GreedyAttribute();
            // Act & assert
            await Assert.That(() =>
                sut.GetCustomization(null)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new GreedyAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), [typeof(object)])!
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var invoker = Assert.That(result).IsAssignableFrom<ConstructorCustomization>();
            Assert.That(invoker.TargetType).IsEqualTo(parameter.ParameterType);
            Assert.That(invoker.Query).IsAssignableFrom<GreedyConstructorQuery>();
        }
    }
}
