using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    class FavorArraysAttributeTests
    {
        [Test]
        public void SutIsAttribute()
        {
            var sut = new FavorArraysAttribute();

            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            var sut = new FavorArraysAttribute();

            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            var sut = new FavorArraysAttribute();
            var parameter = typeof(TypeWithMembers).GetMethod("DoSomething").GetParameters().Single();
            
            var result = sut.GetCustomization(parameter);

            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization) result;
            Assert.That(parameter.ParameterType, Is.EqualTo(invoker.TargetType));
            Assert.IsInstanceOf<ArrayFavoringConstructorQuery>(invoker.Query);
        }
    }
}
