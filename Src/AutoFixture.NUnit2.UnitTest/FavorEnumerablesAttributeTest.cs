using System;
using System.Linq;
using AutoFixture.Kernel;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
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
            // Fixture setup
            var sut = new FavorEnumerablesAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            Assert.AreEqual(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<EnumerableFavoringConstructorQuery>(invoker.Query);
            // Teardown
        }
    }
}
