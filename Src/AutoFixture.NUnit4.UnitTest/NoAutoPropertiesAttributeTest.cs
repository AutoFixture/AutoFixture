using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit4.UnitTest
{
    [TestFixture]
    public class NoAutoPropertiesAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new NoAutoPropertiesAttribute();
            // Assert
            Assert.That(sut, Is.InstanceOf<CustomizeAttribute>());
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
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
            Assert.That(result, Is.AssignableFrom<NoAutoPropertiesCustomization>());
        }
    }
}
