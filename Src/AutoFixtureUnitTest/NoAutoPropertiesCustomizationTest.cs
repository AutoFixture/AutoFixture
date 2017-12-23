using System;
using AutoFixture;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class NoAutoPropertiesCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            var dummy = typeof(object);
            // Act
            var sut = new NoAutoPropertiesCustomization(dummy);
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullTargetTypeThrowsArgumentNullException()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new NoAutoPropertiesCustomization(null));
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var dummy = typeof(object);
            var sut = new NoAutoPropertiesCustomization(dummy);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeCorrectlyDisablesAutoPropertyPopulationForType()
        {
            // Arrange
            var targetType = typeof(PropertyHolder<string>);
            var fixture = new Fixture();
            var sut = new NoAutoPropertiesCustomization(targetType);
            // Act
            var fixtureBeforeCustomization = fixture.Create<PropertyHolder<string>>();
            sut.Customize(fixture);
            var fixtureAfterCustomization = fixture.Create<PropertyHolder<string>>();
            var secondFixtureAfterCustomization = fixture.Create<PropertyHolder<string>>();
            // Assert
            Assert.NotNull(fixtureBeforeCustomization.Property);
            Assert.Null(fixtureAfterCustomization.Property);
            Assert.Null(secondFixtureAfterCustomization.Property);
        }
    }
}
