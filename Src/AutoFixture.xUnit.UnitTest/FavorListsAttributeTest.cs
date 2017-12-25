using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit.UnitTest
{
    public class FavorListsAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new FavorListsAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new FavorListsAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new FavorListsAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<ListFavoringConstructorQuery>(invoker.Query);
        }
    }
}
