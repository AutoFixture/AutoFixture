using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CompositeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new CompositeCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeCustomization((ICustomization[])null));
        }

        [Fact]
        public void CustomizationsIsCorrectWhenInitializedWithArray()
        {
            // Arrange
            var customizations = new[]
            {
                new DelegatingCustomization(),
                new DelegatingCustomization(),
                new DelegatingCustomization()
            };

            var sut = new CompositeCustomization(customizations);
            // Act
            IEnumerable<ICustomization> result = sut.Customizations;
            // Assert
            Assert.True(customizations.SequenceEqual(result));
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeCustomization((IEnumerable<ICustomization>)null));
        }

        [Fact]
        public void CustomizationsIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            var customizations = new[]
            {
                new DelegatingCustomization(),
                new DelegatingCustomization(),
                new DelegatingCustomization()
            };

            var sut = new CompositeCustomization(customizations);
            // Act
            var result = sut.Customizations;
            // Assert
            Assert.True(customizations.SequenceEqual(result));
        }

        [Fact]
        public void CustomizeCustomizesFixtureForAllCustomizations()
        {
            // Arrange
            var fixture = new Fixture();

            var verifications = new List<bool>();
            var customizations = new[]
            {
                new DelegatingCustomization { OnCustomize = f => verifications.Add(f == fixture)},
                new DelegatingCustomization { OnCustomize = f => verifications.Add(f == fixture)},
                new DelegatingCustomization { OnCustomize = f => verifications.Add(f == fixture)}
            };

            var sut = new CompositeCustomization(customizations);
            // Act
            sut.Customize(fixture);
            // Assert
            Assert.Equal(customizations.Length, verifications.Count);
            Assert.True(verifications.All(b => b));
        }
    }
}
