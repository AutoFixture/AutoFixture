using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class ModestAttributeTests
    {
        [Test]
        public void SutIsAttribute()
        {
            var sut = new ModestAttribute();

            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParamterThrows()
        {
            var sut = new ModestAttribute();
            
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            var sut = new ModestAttribute();
            var parameter = typeof(TypeWithMembers).GetMethod("DoSomething").GetParameters().Single();
            
            var result = sut.GetCustomization(parameter);

            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var customization = (ConstructorCustomization) result;
            Assert.That(parameter.ParameterType, Is.EqualTo(customization.TargetType));
            Assert.IsInstanceOf<ModestConstructorQuery>(customization.Query);
        }
    }
}
