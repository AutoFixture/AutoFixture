using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class FavorEnumerablesAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FavorEnumerablesAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
            // Teardown
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new FavorEnumerablesAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            var sut = new FavorEnumerablesAttribute();
            var parameter = typeof(TypeWithMembers).GetMethod("DoSomething").GetParameters().Single();
            
            var result = sut.GetCustomization(parameter);
            
            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            Assert.That(parameter.ParameterType, Is.EqualTo(invoker.TargetType));
            Assert.IsAssignableFrom<EnumerableFavoringConstructorQuery>(invoker.Query);
        }
    }
}
