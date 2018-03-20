using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoNSubstituteCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new AutoNSubstituteCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeMembersIsDisabledByDefault()
        {
            // Arrange
            // Act
            var sut = new AutoNSubstituteCustomization();
            // Assert
            Assert.False(sut.ConfigureMembers);
        }

        [Fact, Obsolete]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoNSubstituteCustomization(null));
        }

        [Fact]
        public void ThrowsIfNullRelayIsSet()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.Relay = null);
        }

        [Fact]
        public void ShouldPreserveTheSetRelay()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            var relay = new CompositeSpecimenBuilder();
            // Act
            sut.Relay = relay;
            // Assert
            Assert.Equal(relay, sut.Relay);
        }
        [Fact, Obsolete]
        public void SpecificationIsCorrectWhenInitializedWithBuilder()
        {
            // Arrange
            var expectedBuilder = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>());
            var sut = new AutoNSubstituteCustomization(expectedBuilder);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact, Obsolete]
        public void BuilderIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act
            var result = sut.Builder;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void RelayIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act
            var result = sut.Relay;
            // Assert
            Assert.NotNull(result);
        }

        [Fact, Obsolete]
        public void BuilderIsSubstituteRelay_WhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act
            var result = sut.Builder;
            // Assert
            Assert.IsType<SubstituteRelay>(result);
        }

        [Fact]
        public void RelayIsSubstituteRelay_WhenInitializedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act
            var result = sut.Relay;
            // Assert
            Assert.IsType<SubstituteRelay>(result);
        }
        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsAppropriateResidueCollector()
        {
            // Arrange
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = Substitute.For<IFixture>();
            fixtureStub.ResidueCollectors.Returns(residueCollectors);

            var sut = new AutoNSubstituteCustomization();
            // Act
            sut.Customize(fixtureStub);
            // Assert
            Assert.Contains(sut.Relay, residueCollectors);
        }

        [Fact]
        public void CustomizeInsertsSubstituteAttributeRelayInCustomizationsToOverrideDefaultConstructionWhenAttributeIsPresent()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            var fixture = Substitute.For<IFixture>();
            // Act
            sut.Customize(fixture);
            // Assert
            fixture.Customizations.Received().Insert(0, Arg.Any<SubstituteAttributeRelay>());
        }

        [Fact]
        public void CustomizeInsertsProperlyConfiguredSubstituteRequestHandlerInCustomizationsToHandleSubstituteRequests()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            var fixtureStub = new FixtureStub();
            // Act
            sut.Customize(fixtureStub);
            // Assert
            var substituteRequestHandler = fixtureStub.Customizations.OfType<SubstituteRequestHandler>().Single();
            var substituteConstructor = Assert.IsType<MethodInvoker>(substituteRequestHandler.SubstituteFactory);
            Assert.IsType<NSubstituteMethodQuery>(substituteConstructor.Query);
        }

        [Fact]
        public void WithConfigureMembers_CustomizeAddsPostprocessorWithSubstituteRequestHandlerAndCommandsToCustomizations()
        {
            // Arrange
            var fixtureStub = new FixtureStub();
            var sut = new AutoNSubstituteCustomization { ConfigureMembers = true };
            // Act
            sut.Customize(fixtureStub);
            // Assert
            var postprocessor = fixtureStub.Customizations.OfType<Postprocessor>().Single();
            var substituteRequestHandler = Assert.IsAssignableFrom<SubstituteRequestHandler>(postprocessor.Builder);
            var substituteFactory = Assert.IsType<MethodInvoker>(substituteRequestHandler.SubstituteFactory);
            Assert.IsType<NSubstituteMethodQuery>(substituteFactory.Query);
            var compositeCommand = Assert.IsAssignableFrom<CompositeSpecimenCommand>(postprocessor.Command);
            Assert.True(compositeCommand.Commands.OfType<NSubstituteRegisterCallHandlerCommand>().Any());
            Assert.True(compositeCommand.Commands.OfType<NSubstituteSealedPropertiesCommand>().Any());
        }
    }
}