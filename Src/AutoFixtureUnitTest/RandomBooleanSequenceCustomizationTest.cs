using System;
using Ploeh.AutoFixture;
using Xunit;
using System.Linq;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomBooleanSequenceCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Exercise system
            var sut = new RandomBooleanSequenceCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new RandomBooleanSequenceCustomization();
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
            var sut = new RandomBooleanSequenceCustomization();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome (no exception from Single indicates success)
            fixture.Customizations.OfType<RandomBooleanSequenceGenerator>().Single();
            // Teardown
        }


    }
}