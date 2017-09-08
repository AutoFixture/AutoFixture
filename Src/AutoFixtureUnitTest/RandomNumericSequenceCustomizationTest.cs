using Ploeh.AutoFixture;
using System;
using System.Linq;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomNumericSequenceCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomNumericSequenceCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullFixtureThrows()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCorrectBuilderToFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new RandomNumericSequenceCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<RandomNumericSequenceGenerator>()
                .SingleOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }
    }
}