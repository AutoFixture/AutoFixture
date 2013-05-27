using System;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SupportStructureDefaultConstructorsCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Exercise system
            var sut = new SupportStructureDefaultConstructorsCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new SupportStructureDefaultConstructorsCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeProperFixtureCorrectlyCustomizesIt()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new SupportStructureDefaultConstructorsCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = fixture.Customizations.OfType<SupportStructureDefaultConstructorsGenerator>().SingleOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        } 
    }
}