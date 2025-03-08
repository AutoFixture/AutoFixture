using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest;

public class FrozenAttributeTest
{
    [Test]
    public async Task SutIsAttribute()
    {
        // Arrange
        // Act
        var sut = new FrozenAttribute();
        // Assert
        await Assert.That(sut).IsAssignableTo<CustomizeAttribute>();
    }

    [Test]
    public async Task GetCustomizationFromNullParameterThrows()
    {
        // Arrange
        var sut = new FrozenAttribute();
        // Act & assert
        await Assert.That(() =>
            sut.GetCustomization(null)).ThrowsExactly<ArgumentNullException>();
    }
}