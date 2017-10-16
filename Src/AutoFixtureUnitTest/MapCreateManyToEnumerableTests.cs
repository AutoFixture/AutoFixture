using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class MapCreateManyToEnumerableTests
    {
        [Fact]
        public void SutIsCustomization()
        {
            var sut = new MapCreateManyToEnumerable();
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeAddsCorrectSpecimenBuilderToFixture()
        {
            // Fixture setup
            var sut = new MapCreateManyToEnumerable();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(
                fixture.Customizations.OfType<MultipleToEnumerableRelay>().Any(),
                "Appropriate SpecimenBuilder should be added to Fixture.");
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new MapCreateManyToEnumerable();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
            // Teardown
        }
    }
}
