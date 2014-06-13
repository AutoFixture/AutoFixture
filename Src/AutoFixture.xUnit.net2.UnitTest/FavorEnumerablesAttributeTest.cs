using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    public class FavorEnumerablesAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FavorEnumerablesAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new FavorEnumerablesAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Fact]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FavorEnumerablesAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            var invoker = Assert.IsAssignableFrom<ConstructorCustomization>(result);
            Assert.Equal(parameter.ParameterType, invoker.TargetType);
            Assert.IsAssignableFrom<EnumerableFavoringConstructorQuery>(invoker.Query);
            // Teardown
        }
    }
}
