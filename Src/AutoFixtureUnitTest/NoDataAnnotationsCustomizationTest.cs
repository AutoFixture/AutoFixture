using System;
using System.Linq;
using AutoFixture;
using AutoFixture.DataAnnotations;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class NoDataAnnotationsCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Exercise system
            var sut = new NoDataAnnotationsCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new NoDataAnnotationsCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(RangeAttributeRelay))]
        [InlineData(typeof(StringLengthAttributeRelay))]
        [InlineData(typeof(RegularExpressionAttributeRelay))]
        [InlineData(typeof(NumericRangedRequestRelay))]
        public void CustomizeProperFixtureCorrectlyCustomizesIt(Type removedBuilderType)
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new NoDataAnnotationsCustomization();
            // Exercise system
            sut.Customize(fixture);

            // Verify outcome
            Assert.DoesNotContain(
                fixture.Customizations,
                b => b.GetType() == removedBuilderType);
            // Teardown
        }
    }
}