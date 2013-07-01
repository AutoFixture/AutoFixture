using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
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
    }
}
