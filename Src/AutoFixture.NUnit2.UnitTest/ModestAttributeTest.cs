using System;
using System.Linq;
using AutoFixture.Kernel;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class ModestAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new ModestAttribute();
            // Assert
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Arrange
            var sut = new ModestAttribute();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new ModestAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            Assert.IsAssignableFrom<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            Assert.AreEqual(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<ModestConstructorQuery>(invoker.Query);
        }
    }
}
