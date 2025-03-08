using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest;

public class FavorEnumerablesAttributeTest
{
    [Test]
    public async Task SutIsAttribute()
    {
        // Arrange
        // Act
        var sut = new FavorEnumerablesAttribute();
        // Assert
        await Assert.That(sut).IsAssignableTo<CustomizeAttribute>();
    }

    [Test]
    public async Task GetCustomizationFromNullParameterThrows()
    {
        // Arrange
        var sut = new FavorEnumerablesAttribute();
        // Act & assert
        await Assert.That(() =>
            sut.GetCustomization(null)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task GetCustomizationReturnsCorrectResult()
    {
        // Arrange
        var sut = new FavorEnumerablesAttribute();
        var parameter = typeof(TypeWithOverloadedMembers)
            .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething), [typeof(object)])!
            .GetParameters().Single();
        // Act
        var result = sut.GetCustomization(parameter);
        // Assert
        var invoker = await Assert.That(result).IsAssignableTo<ConstructorCustomization>();
        await Assert.That(invoker.TargetType).IsEqualTo(parameter.ParameterType);
        await Assert.That(invoker.Query).IsAssignableTo<EnumerableFavoringConstructorQuery>();
    }
}