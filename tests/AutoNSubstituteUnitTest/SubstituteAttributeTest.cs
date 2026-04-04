using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteAttributeTest
    {
        [Fact]
        public void ClassIsAnAttribute()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(typeof(Attribute).IsAssignableFrom(typeof(SubstituteAttribute)));
        }

        [Theory]
        [InlineData(AttributeTargets.Parameter)]
        [InlineData(AttributeTargets.Property)]
        [InlineData(AttributeTargets.Field)]
        public void AttributeCanBeAppliedToCodeElementsSupportedBySubstituteAttributeRelay(AttributeTargets expectedTarget)
        {
            // Arrange
            var attributeUsage = typeof(SubstituteAttribute).GetTypeInfo().GetCustomAttributes(false)
                .OfType<AttributeUsageAttribute>().Single();
            // Act
            Assert.Equal(expectedTarget, attributeUsage.ValidOn & expectedTarget);
            // Assert
        }

        [Fact]
        public void AttributeCanBeAppliedOnlyOnceBecauseDefiningMultipleSubstitutesForSingleArgumentIsMeaningless()
        {
            // Arrange
            var attributeUsage = typeof(SubstituteAttribute).GetTypeInfo().GetCustomAttributes(false)
                .OfType<AttributeUsageAttribute>().Single();
            // Act
            Assert.False(attributeUsage.AllowMultiple);
            // Assert
        }
    }
}
