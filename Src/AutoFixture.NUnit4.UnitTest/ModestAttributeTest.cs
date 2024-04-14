using System;
using System.Linq;
using AutoFixture.Kernel;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using TestTypeFoundation;

namespace AutoFixture.NUnit4.UnitTest
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
            ClassicAssert.IsInstanceOf<CustomizeAttribute>(sut);
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
            ClassicAssert.IsAssignableFrom<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            ClassicAssert.AreEqual(parameter.ParameterType, invoker.TargetType);
            ClassicAssert.IsAssignableFrom<ModestConstructorQuery>(invoker.Query);
        }
    }
}
