using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.DataAnnotations;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RangeAttributeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new RangeAttributeCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new RangeAttributeCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCorrectItemsToCustomizations()
        {
            // Fixture setup
            var sut = new RangeAttributeCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.Customizations.OfType<RangeAttributeRelay>().Any());
            Assert.True(fixture.Customizations.OfType<RangedNumberGenerator>().Any());
            // Teardown
        }
    }
}
