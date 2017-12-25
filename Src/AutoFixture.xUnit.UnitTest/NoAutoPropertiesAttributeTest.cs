using System;
using System.Linq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit.UnitTest
{
    public class NoAutoPropertiesAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new NoAutoPropertiesAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
        public void GetCustomizationReturnsTheCorrectResult()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object) })
                .GetParameters()
                .Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            Assert.IsAssignableFrom<NoAutoPropertiesCustomization>(result);
        }
    }
}
