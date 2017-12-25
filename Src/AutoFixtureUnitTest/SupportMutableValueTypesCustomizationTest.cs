using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SupportMutableValueTypesCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Act
            var sut = new SupportMutableValueTypesCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new SupportMutableValueTypesCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeProperFixtureCorrectlyCustomizesIt()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new SupportMutableValueTypesCustomization();
            // Act
            sut.Customize(fixture);

            var results = fixture.Customizations
                                 .OfType<Postprocessor>()
                                 .Where(
                                     b =>
                                     b.Builder is MutableValueTypeGenerator)
                                 .Where(b => b.Command is AutoPropertiesCommand)
                                 .SingleOrDefault();
            // Assert
            Assert.NotNull(results);
        }
    }
}