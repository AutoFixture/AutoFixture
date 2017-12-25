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
            // Arrange
            var sut = new MapCreateManyToEnumerable();
            var fixture = new Fixture();
            // Act
            sut.Customize(fixture);
            // Assert
            Assert.True(
                fixture.Customizations.OfType<MultipleToEnumerableRelay>().Any(),
                "Appropriate SpecimenBuilder should be added to Fixture.");
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new MapCreateManyToEnumerable();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
        }
    }
}
