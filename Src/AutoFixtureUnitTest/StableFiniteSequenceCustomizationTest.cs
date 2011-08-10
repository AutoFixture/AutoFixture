using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class StableFiniteSequenceCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new StableFiniteSequenceCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new StableFiniteSequenceCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCorrectItemToCustomizations()
        {
            // Fixture setup
            var sut = new StableFiniteSequenceCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.Customizations.OfType<StableFiniteSequenceRelay>().Any());
            // Teardown
        }
    }
}
