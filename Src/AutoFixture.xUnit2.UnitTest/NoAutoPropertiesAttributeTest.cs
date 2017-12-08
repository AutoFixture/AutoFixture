using System;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest
{
    public class NoAutoPropertiesAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new NoAutoPropertiesAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new NoAutoPropertiesAttribute();
            // Exercise system and verify the outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
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
            Assert.IsAssignableFrom<NoAutoPropertiesCustomization>(result);
        }
    }
}
