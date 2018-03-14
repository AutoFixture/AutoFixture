using System;
using System.Linq;
using AutoFixture.Kernel;
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
        public void ConfigureMembersIsDisabledByDefault()
        {
            // Arrange
            // Act
            var sut = new AutoMoqCustomization();
            // Assert
            Assert.False(sut.ConfigureMembers);
        }

        [Fact, Obsolete]
        public void InitializeWithNullRelayThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoMoqCustomization(null));
        }

        [Fact]
        public void ThrowsIfNullRelayIsSet()
        {
            // Arrange
            var sut = new AutoMoqCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Relay = null);
        }

        [Fact]
        public void ShouldPreserveTheSetRelay()
        {
            // Arrange
            var sut = new AutoMoqCustomization();
            var relay = new CompositeSpecimenBuilder();
            // Act
            sut.Relay = relay;
            // Assert
            Assert.Equal(relay, sut.Relay);
        }

        [Fact, Obsolete]
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
            var fixtureStub = new FixtureStub();

            var sut = new AutoMoqCustomization();
            // Act
            sut.Customize(fixtureStub);
            // Assert
            Assert.Contains(sut.Relay, fixtureStub.ResidueCollectors);
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Arrange
            var fixtureStub = new FixtureStub();

            var sut = new AutoMoqCustomization();
            // Act
            sut.Customize(fixtureStub);
            // Assert
            var postprocessor = fixtureStub.Customizations.OfType<MockPostprocessor>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<MockConstructorQuery>(ctorInvoker.Query);
        }

        [Fact]
        public void WithConfigureMembers_CustomizeAddsPostprocessorToCustomizations()
        {
            // Arrange
            var fixtureStub = new FixtureStub();
            var sut = new AutoMoqCustomization { ConfigureMembers = true };
            // Act
            sut.Customize(fixtureStub);
            // Assert
            Assert.Contains(fixtureStub.Customizations, builder => builder is Postprocessor);
        }

        [Theory]
        [InlineData(typeof(MockVirtualMethodsCommand))]
        [InlineData(typeof(StubPropertiesCommand))]
        [InlineData(typeof(AutoMockPropertiesCommand))]
        public void WithConfigureMembers_CustomizeAddsMockCommandsToPostprocessor(Type expectedCommandType)
        {
            // Arrange
            var fixtureStub = new FixtureStub();
            var sut = new AutoMoqCustomization { ConfigureMembers = true };
            // Act
            sut.Customize(fixtureStub);
            // Assert
            var postprocessor = (Postprocessor) fixtureStub.Customizations.Single(builder => builder is Postprocessor);
            var compositeCommand = (CompositeSpecimenCommand) postprocessor.Command;

            Assert.Contains(compositeCommand.Commands, command => command.GetType() == expectedCommandType);
        }
    }
}
