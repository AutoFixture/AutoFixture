using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
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
            var result = fixture.Customizations.OfType<Postprocessor>().SingleOrDefault();

            var results = fixture.Customizations
                                 .OfType<Postprocessor>()
                                 .Where(
                                     b =>
                                     b.Builder is SupportStructureDefaultConstructorsGenerator)
                                 .Where(b => b.Command is AutoPropertiesCommand)
                                 .SingleOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        } 
    }
}