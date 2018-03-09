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

        [Fact, Obsolete]
        public void InitializeWithNullRelayThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoFakeItEasyCustomization(null));
        }

        [Fact]
        public void SetNullRelayThrows()
        {
            // Arrange
            var sut = new AutoFakeItEasyCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Relay = null);
        }

        [Fact]
        public void DelegatesFeatureIsDisabledByDefault()
        {
            // Arrange
            // Act
            var sut = new AutoFakeItEasyCustomization();
            // Assert
            Assert.False(sut.GenerateDelegates);
        }

#if !CAN_FAKE_DELEGATES
        [Fact]
        public void InitializeWithGenerateDelegatesOptionThrowsIfNotSupported()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                new Fixture().Customize(new AutoFakeItEasyCustomization { GenerateDelegates = true }));
            // Assert
            Assert.Contains(
                "Option GenerateDelegates was specified, but this requires FakeItEasy version 1.7.4257.42 or higher",
                ex.Message);
        }
#endif

        [Fact, Obsolete]
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
        public void SpecificationIsCorrectWhenRelayIsSetViaProperty()
        {
            // Arrange
            var expectedRelay = new FakeItEasyRelay();
            // Act
            var sut = new AutoFakeItEasyCustomization { Relay = expectedRelay };
            // Assert
            ISpecimenBuilder result = sut.Relay;
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