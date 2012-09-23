using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
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
        public void CustomizeAddsCorrectBuildersToFixture()
        {
            // Fixture setup
            var expectedResult = new[]
            {
                typeof(RandomNumericSequenceLimitGenerator),
                typeof(RandomNumericSequenceGenerator)
            };
            var fixture = new Fixture();
            var sut = new RandomNumericSequenceCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<CompositeSpecimenBuilder>()
                .SelectMany(i => i.Builders)
                .Select(i => i.GetType());
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }
    }
}