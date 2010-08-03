using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class FrozenAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FrozenAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Fact]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            var parameter = this.GetType().GetMethod("Ploeh").GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(parameter.ParameterType, freezer.TargetType);
            // Teardown
        }

        public void Ploeh(string fnaah)
        {
        }
    }
}
