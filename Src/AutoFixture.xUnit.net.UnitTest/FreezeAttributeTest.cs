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
        public void GetCustomizationShouldReturnFreezeOnMatchCustomization()
        {
            // Fixture setup
            var sut = new FreezeAttribute();
            // Exercise system
            var customization = sut.GetCustomization(AParameter<ConcreteType>());
            // Verify outcome
            Assert.IsAssignableFrom<FreezeOnMatchCustomization<ConcreteType>>(customization);
            // Teardown
        }

        [Fact]
        public void GetCustomizationShouldReturnFreezeOnMatchCustomizationWithDefaultSettings()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FreezeAttribute();
            // Exercise system
            var customization = (FreezeOnMatchCustomization<object>)sut.GetCustomization(parameter);
            // Verify outcome
            Assert.Equal(sut.By, customization.MatchBy);
            Assert.Empty(customization.TargetNames);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingStrategyShouldReturnFreezeOnMatchCustomizationWithThatStrategyAndExactType()
        {
            // Fixture setup
            var strategy = Matching.BaseType;
            var sut = new FreezeAttribute { By = strategy };
            // Exercise system
            var customization = (FreezeOnMatchCustomization<object>)sut.GetCustomization(AParameter<object>());
            // Verify outcome
            Assert.Equal(strategy | Matching.ExactType, customization.MatchBy);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMemberNameShouldReturnFreezeOnMatchCustomizationWithThatName()
        {
            // Fixture setup
            var name = "SomeName";
            var sut = new FreezeAttribute { TargetName = name };
            // Exercise system
            var customization = (FreezeOnMatchCustomization<object>)sut.GetCustomization(AParameter<object>());
            // Verify outcome
            Assert.Contains(name, customization.TargetNames);
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

        private static ParameterInfo AParameter<T>()
        {
            return typeof(SingleParameterType<T>)
                .GetConstructor(new[] { typeof(T) })
                .GetParameters()
                .Single(p => p.Name == "parameter");
        }
    }
}
