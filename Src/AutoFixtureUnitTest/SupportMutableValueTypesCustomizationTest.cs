using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SupportMutableValueTypesCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Exercise system
            var sut = new SupportMutableValueTypesCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new SupportMutableValueTypesCustomization();
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
            var sut = new SupportMutableValueTypesCustomization();
            // Exercise system
            sut.Customize(fixture);

            var results = fixture.Customizations
                                 .OfType<Postprocessor>()
                                 .Where(
                                     b =>
                                     b.Builder is MutableValueTypeGenerator)
                                 .Where(b => b.Command is AutoPropertiesCommand)
                                 .SingleOrDefault();
            // Verify outcome
            Assert.NotNull(results);
            // Teardown
        } 
    }
}