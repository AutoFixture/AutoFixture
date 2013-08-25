using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.NUnit;
using Ploeh.TestTypeFoundation;

namespace Ploe.AutoFixture.NUnit.UnitTest
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
            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization)result;
            Assert.AreSame(parameter.ParameterType, invoker.TargetType);
            Assert.IsInstanceOf<EnumerableFavoringConstructorQuery>(invoker.Query);
            // Teardown
        }
    }
}
