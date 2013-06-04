
using Xunit;
using Ploeh.AutoFixture;
using System;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    public class NoAutoPropertiesCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            var dummy = typeof(object);
            // Exercise system
            var sut = new NoAutoPropertiesCustomization(dummy);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullTargetTypeThrowsArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify the outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoAutoPropertiesCustomization(null));
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var dummy = typeof(object);
            var sut = new NoAutoPropertiesCustomization(dummy);
            // Exercise system and verify the outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeCorrectlyDisablesAutoPropertyPopulationForType()
        {
            // Fixture setup
            var targetType = typeof(PropertyHolder<TheAutoProperty>);
            var fixture = new Fixture();
            var sut = new NoAutoPropertiesCustomization(targetType);
            // Exercise system
            sut.Customize(fixture);
            var propertyHolder = fixture.Create<PropertyHolder<TheAutoProperty>>();
            var propertyHolder2 = fixture.Create<PropertyHolder<TheAutoProperty>>();
            // Verify the outcome
            Assert.Equal(null, propertyHolder.Property);
            Assert.Equal(null, propertyHolder2.Property);
        }
    }

    public class TheAutoProperty
    {
    }
}
