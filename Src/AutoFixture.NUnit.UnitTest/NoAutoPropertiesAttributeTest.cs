using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit;
using Ploeh.TestTypeFoundation;

namespace Ploe.AutoFixture.NUnit.UnitTest
{
    [TestFixture]
    public class NoAutoPropertiesAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new NoAutoPropertiesAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new NoAutoPropertiesAttribute();
            // Exercise system and verify the outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsTheCorrectResult()
        {
            // Fixture setup
            var sut = new NoAutoPropertiesAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify the outcome
            Assert.IsInstanceOf<NoAutoPropertiesCustomization>(result);
        }
    }
}
