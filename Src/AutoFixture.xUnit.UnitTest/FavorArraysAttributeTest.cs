using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit.UnitTest
{
    public class FavorArraysAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FavorArraysAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new FavorArraysAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Fact]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FavorArraysAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<ArrayFavoringConstructorQuery>(invoker.Query);
            // Teardown
        }
    }
}
