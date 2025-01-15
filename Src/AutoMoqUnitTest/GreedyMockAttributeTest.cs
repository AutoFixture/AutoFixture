using System;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class GreedyMockAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new GreedyMockAttribute();
            // Assert
            Assert.IsAssignableFrom<MockCustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Arrange
            var sut = new GreedyMockAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new GreedyMockAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<GreedyMockConstructorQuery>(invoker.Query);
        }
    }
}
