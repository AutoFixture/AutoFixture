using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    [TestFixture]
    public class ModestAttributeTest
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
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            Assert.AreSame(parameter.ParameterType, invoker.TargetType);
            Assert.IsInstanceOf<ModestConstructorQuery>(invoker.Query);
            // Teardown
        }
    }
}
