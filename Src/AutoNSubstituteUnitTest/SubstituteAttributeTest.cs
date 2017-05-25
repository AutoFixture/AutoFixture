using System;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteAttributeTest
    {
        [Fact]
        public void ClassIsAnAttribute()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(Attribute).IsAssignableFrom(typeof(SubstituteAttribute)));
            // Teardown
        }

        [Theory]
        [InlineData(AttributeTargets.Parameter)]
        [InlineData(AttributeTargets.Property)]
        [InlineData(AttributeTargets.Field)]
        public void AttributeCanBeAppliedToCodeElementsSupportedBySubstituteAttributeRelay(AttributeTargets expectedTarget)
        {
            // Fixture setup
            var attributeUsage = typeof(SubstituteAttribute).GetCustomAttributes(false)
                .OfType<AttributeUsageAttribute>().Single();
            // Exercise system
            Assert.Equal(expectedTarget, attributeUsage.ValidOn & expectedTarget);
            // Verify outcome
            // Teardown
        }

        [Fact]
        public void AttributeCanBeAppliedOnlyOnceBecauseDefiningMultipleSubstitutesForSingleArgumentIsMeaningless()
        {
            // Fixture setup
            var attributeUsage = typeof(SubstituteAttribute).GetCustomAttributes(false)
                .OfType<AttributeUsageAttribute>().Single();
            // Exercise system
            Assert.False(attributeUsage.AllowMultiple);
            // Verify outcome
            // Teardown
        }

        [Fact]
        public void SutIsCustomizeAttribute()
        {
            var sut = new SubstituteAttribute();
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationWithNullTypeThrows()
        {
            var sut = new SubstituteAttribute();
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
        public void GetCustomizationReturnsConcreteClassNSubstituteCustomizationWithValidType()
        {
            // Fixture setup
            var parameter = typeof(object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance)
                .GetParameters().Single();
            var expectedType = typeof(object);
            var sut = new SubstituteAttribute();
            // Exercise system
            var result = (ConcreteClassNSubstituteCustomization)sut.GetCustomization(parameter);
            // Verify outcome
            Assert.Equal(expectedType, result.TypeToProxy);
            // Teardown
        }
    }
}
