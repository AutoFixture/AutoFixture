using System;
using System.Linq;
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
    }
}
