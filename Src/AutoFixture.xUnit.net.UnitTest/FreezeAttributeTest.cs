using System;
using System.Linq;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class FreezeAttributeTest
    {
        [Fact]
        public void SutIsCustomizeAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FreezeAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeShouldSetDefaultMatchingStrategy()
        {
            // Fixture setup
            // Exercise system
            var sut = new FreezeAttribute();
            // Verify outcome
            Assert.Equal(Matching.ExactType, sut.By);
            // Teardown
        }

        [Fact]
        public void InitializeShouldSetDefaultTargetName()
        {
            // Fixture setup
            // Exercise system
            var sut = new FreezeAttribute();
            // Verify outcome
            Assert.Null(sut.TargetName);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithNullShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new FreezeAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.GetCustomization(null));
        }

        [Fact]
        public void GetCustomizationShouldReturnFreezeOnMatchCustomization()
        {
            // Fixture setup
            var sut = new FreezeAttribute();
            // Exercise system
            var customization = sut.GetCustomization(Parameter<string>());
            // Verify outcome
            Assert.IsAssignableFrom<FreezeOnMatchCustomization<string>>(customization);
            // Teardown
        }

        [Fact]
        public void GetCustomizationShouldAlwaysFreezeByMatchingTheSpecifiedParameter()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeAttribute { By = Matching.BaseType };
            // Exercise system
            var customization = sut.GetCustomization(Parameter<string>());
            fixture.Customize(customization);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<string>>().Parameter;
            var requested = fixture.Create<SingleParameterType<string>>().Parameter;
            Assert.Same(frozen, requested);
            // Teardown
        }

        private static ParameterInfo Parameter<T>()
        {
            return typeof(SingleParameterType<T>)
                .GetConstructor(new[] { typeof(T) })
                .GetParameters()
                .Single(p => p.Name == "parameter");
        }
    }
}
