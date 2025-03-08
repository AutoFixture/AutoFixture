using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;
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
            await Assert.That(sut).IsAssignableTo<CustomizeAttribute>();
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
        public async Task GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new GreedyAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), [typeof(object)])!
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var invoker = await Assert.That(result).IsAssignableTo<ConstructorCustomization>();
            await Assert.That(invoker.TargetType).IsEqualTo(parameter.ParameterType);
            await Assert.That(invoker.Query).IsAssignableTo<GreedyConstructorQuery>();
        }
    }
}
