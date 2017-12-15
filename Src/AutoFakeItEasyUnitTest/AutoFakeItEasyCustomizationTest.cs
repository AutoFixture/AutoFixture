using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using FakeItEasy;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class AutoFakeItEasyCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new AutoFakeItEasyCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullRelayThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoFakeItEasyCustomization(null));
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithRelay()
        {
            // Arrange
            var expectedRelay = new FakeItEasyRelay();
            var sut = new AutoFakeItEasyCustomization(expectedRelay);
            // Act
            ISpecimenBuilder result = sut.Relay;
            // Assert
            Assert.Equal(expectedRelay, result);
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AutoFakeItEasyCustomization();
            // Act
            var result = sut.Relay;
            // Assert
            Assert.IsType<FakeItEasyRelay>(result);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new AutoFakeItEasyCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Arrange
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = new Fake<IFixture>();
            fixtureStub.CallsTo(c => c.Customizations).Returns(customizations);

            var sut = new AutoFakeItEasyCustomization();
            // Act
            sut.Customize(fixtureStub.FakedObject);
            // Assert
            var postprocessor = customizations.OfType<FakeItEasyBuilder>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<FakeItEasyMethodQuery>(ctorInvoker.Query);
        }

        [Fact]
        public void CustomizeAddsAppropriateResidueCollector()
        {
            // Arrange
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = new Fake<IFixture>();
            fixtureStub.CallsTo(c => c.ResidueCollectors).Returns(residueCollectors);
            
            var sut = new AutoFakeItEasyCustomization();
            // Act
            sut.Customize(fixtureStub.FakedObject);
            // Assert
            Assert.Contains(sut.Relay, residueCollectors);
        }
    }
}