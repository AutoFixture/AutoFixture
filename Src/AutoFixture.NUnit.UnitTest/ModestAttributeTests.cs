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
            // Fixture setup
            // Exercise system
            var sut = new ModestAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
            // Teardown
        }

        [Test]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Fixture setup
            var sut = new ModestAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ModestAttribute();
            var parameter = typeof(TypeWithMembers).GetMethod("DoSomething").GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var customization = (ConstructorCustomization) result;
            Assert.That(parameter.ParameterType, Is.EqualTo(customization.TargetType));
            Assert.IsInstanceOf<ModestConstructorQuery>(customization.Query);
            // Teardown
        }
    }
}
