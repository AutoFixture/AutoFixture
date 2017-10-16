using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class IncrementingDateTimeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new IncrementingDateTimeCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullThrowsArgumentNullException()
        {
            // Fixture setup
            var sut = new IncrementingDateTimeCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsIncrementingDateTimeGeneratorToTheFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new IncrementingDateTimeCustomization();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var containsIncrementingDateTimeGenerator = fixture
                .Customizations
                .OfType<StrictlyMonotonicallyIncreasingDateTimeGenerator>()
                .Any();
            Assert.True(containsIncrementingDateTimeGenerator);
            // Teardown
        }
    }
}
