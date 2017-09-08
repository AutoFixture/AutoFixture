using System;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CurrentDateTimeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new CurrentDateTimeCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullThrowsArgumentNullException()
        {
            // Fixture setup
            var sut = new CurrentDateTimeCustomization();
            // Exercise system and verify outcome
            Assert.Throws(typeof(ArgumentNullException), () => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCurrentDateTimeGeneratorToFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new CurrentDateTimeCustomization();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.Customizations.OfType<CurrentDateTimeGenerator>().Any());
            // Teardown
        }
    }
}