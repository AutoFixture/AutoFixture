using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using TestTypeFoundation;

namespace AutoFixture.NUnit4.UnitTest
{
    [TestFixture]
    public class FavorListsAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new FavorListsAttribute();
            // Assert
            ClassicAssert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new FavorListsAttribute();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Arrange
            var sut = new FavorListsAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            ClassicAssert.IsAssignableFrom<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            ClassicAssert.AreEqual(parameter.ParameterType, invoker.TargetType);
            ClassicAssert.IsAssignableFrom<ListFavoringConstructorQuery>(invoker.Query);
        }
    }
}
