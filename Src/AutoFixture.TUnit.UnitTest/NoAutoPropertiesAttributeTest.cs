using System;
using System.Linq;
using TestTypeFoundation;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest
{
    public class NoAutoPropertiesAttributeTest
    {
        [Test]
        public async Task SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new NoAutoPropertiesAttribute();
            // Assert
            await Assert.That(sut).IsTypeOf<CustomizeAttribute>();
        }

        [Test]
        public async Task GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            // Act & assert
            await Assert.That(() =>
                sut.GetCustomization(null)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task GetCustomizationReturnsTheCorrectResult()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            var parameter = TypeWithOverloadedMembers
                .GetDoSomethingMethod(typeof(object))
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            await Assert.That(result).IsTypeOf<NoAutoPropertiesCustomization>();
        }
    }
}
