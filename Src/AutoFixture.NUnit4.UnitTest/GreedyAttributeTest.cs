using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit4.UnitTest
{
    [TestFixture]
    public class GreedyAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new GreedyAttribute();
            // Assert
            Assert.That(sut, Is.InstanceOf<CustomizeAttribute>());
        }

        [Test]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Arrange
            var sut = new GreedyAttribute();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new GreedyAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            Assert.That(result, Is.AssignableFrom<ConstructorCustomization>());
            var invoker = (ConstructorCustomization)result;
            Assert.That(invoker.TargetType, Is.EqualTo(parameter.ParameterType));
            Assert.That(invoker.Query, Is.AssignableFrom<GreedyConstructorQuery>());
        }
    }
}
