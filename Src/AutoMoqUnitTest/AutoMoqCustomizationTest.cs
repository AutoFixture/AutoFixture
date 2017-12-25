using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class AutoMoqCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new AutoMoqCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullRelayThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoMoqCustomization(null));
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithRelay()
        {
            // Arrange
            var expectedRelay = new MockRelay();
            var sut = new AutoMoqCustomization(expectedRelay);
            // Act
            ISpecimenBuilder result = sut.Relay;
            // Assert
            Assert.Equal(expectedRelay, result);
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AutoMoqCustomization();
            // Act
            var result = sut.Relay;
            // Assert
            Assert.IsType<MockRelay>(result);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new AutoMoqCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsAppropriateResidueCollector()
        {
            // Arrange
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
            fixtureStub.SetupGet(c => c.ResidueCollectors).Returns(residueCollectors);

            var sut = new AutoMoqCustomization();
            // Act
            sut.Customize(fixtureStub.Object);
            // Assert
            Assert.Contains(sut.Relay, residueCollectors);
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Arrange
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
            fixtureStub.SetupGet(c => c.Customizations).Returns(customizations);

            var sut = new AutoMoqCustomization();
            // Act
            sut.Customize(fixtureStub.Object);
            // Assert
            var postprocessor = customizations.OfType<MockPostprocessor>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<MockConstructorQuery>(ctorInvoker.Query);
        }
    }
}
