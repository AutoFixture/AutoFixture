using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CustomizationExtensionsTest
    {
        [Fact]
        public void ToCustomization_ShouldThrowIfBuilderIsNull()
        {
            // Arrange
            ISpecimenBuilder nullBuilder = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                nullBuilder.ToCustomization());
        }

        [Fact]
        public void ToCustomization_ShouldThrowIfNullFixturePassedToCustomization()
        {
            // Arrange
            var sut = new DelegatingSpecimenBuilder().ToCustomization();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(fixture: null));
        }

        [Fact]
        public void ToCustomization_ReturnedCustomizationShouldInsertAtTheBeginning()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            var fixture = new Fixture();

            // Act
            var sut = builder.ToCustomization();
            fixture.Customize(sut);

            // Assert
            Assert.Same(builder, fixture.Customizations[0]);
        }
    }
}