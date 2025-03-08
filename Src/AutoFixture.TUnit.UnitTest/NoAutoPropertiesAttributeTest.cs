using System;
using System.Linq;
using TestTypeFoundation;
using System.Threading.Tasks;

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
            await Assert.That(sut).IsAssignableFrom<CustomizeAttribute>();
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
        public void GetCustomizationReturnsTheCorrectResult()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            var parameter = TypeWithOverloadedMembers
                .GetDoSomethingMethod(typeof(object))
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            Assert.That(result).IsAssignableFrom<NoAutoPropertiesCustomization>();
        }
    }
}
