using System;
using System.Linq;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoConfiguredNSubstituteCustomizationTest
    {
        [Fact]
        public void CtorThrowsWhenRelayIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AutoConfiguredNSubstituteCustomization(null));
        }

        [Fact]
        public void BuilderIsSubstituteRelayByDefault()
        {
            // Arrange
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Act
            var builder = sut.Builder;
            // Assert
            Assert.IsType<SubstituteRelay>(builder);
        }

        [Fact]
        public void CustomizeThrowsWhenFixtureIsNull()
        {
            // Arrange
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Customize(null));
        }

        [Fact]
        public void CustomizeInsertsSubstituteAttributeRelayInCustomizationsToOverrideDefaultConstructionWhenAttributeIsPresent()
        {
            // Arrange
            var sut = new AutoConfiguredNSubstituteCustomization();
            var fixture = Substitute.For<IFixture>();
            // Act
            sut.Customize(fixture);
            // Assert
            fixture.Customizations.Received().Insert(0, Arg.Any<SubstituteAttributeRelay>());
        }

        [Fact]
        public void CustomizeAddsPostprocessorWithSubstituteRequestHandlerAndCommandsToCustomizations()
        {
            // Arrange
            var fixture = Substitute.For<IFixture>();
            Postprocessor postprocessor = null;
            fixture.Customizations.Insert(Arg.Any<int>(), Arg.Do<Postprocessor>(p => postprocessor = p));
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Act
            sut.Customize(fixture);
            // Assert
            var substituteRequestHandler = Assert.IsAssignableFrom<SubstituteRequestHandler>(postprocessor.Builder);
            var substituteFactory = Assert.IsType<MethodInvoker>(substituteRequestHandler.SubstituteFactory);
            Assert.IsType<NSubstituteMethodQuery>(substituteFactory.Query);
            var compositeCommand = Assert.IsAssignableFrom<CompositeSpecimenCommand>(postprocessor.Command);
            Assert.True(compositeCommand.Commands.OfType<NSubstituteRegisterCallHandlerCommand>().Any());
            Assert.True(compositeCommand.Commands.OfType<NSubstituteSealedPropertiesCommand>().Any());
        }

        [Fact]
        public void CustomizeAddsBuilderToResidueCollectors()
        {
            var builder = Substitute.For<ISpecimenBuilder>();
            var fixture = Substitute.For<IFixture>();
            var sut = new AutoConfiguredNSubstituteCustomization(builder);

            sut.Customize(fixture);

            fixture.ResidueCollectors.Received().Add(builder);
        }
    }
}
