using System;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;

namespace AutoFixture.TUnit.UnitTest
{
    public class CustomizeAttributeTest
    {
        [Test]
        public async Task TestableSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            await Assert.That(sut).IsAssignableFrom<CustomizeAttribute>();
        }

        [Test]
        public async Task SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert

            await Assert.That(sut).IsAssignableFrom<Attribute>();
        }

        [Test]
        public async Task SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            await Assert.That(sut).IsAssignableFrom<IParameterCustomizationSource>();
        }
    }
}
