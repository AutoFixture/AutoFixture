using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoConfiguredNSubstituteCustomizationTest
    {
        [Fact]
        public void CtorThrowsWhenRelayIsNull()
        {
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoConfiguredNSubstituteCustomization(null));
        }

        [Fact]
        public void BuilderIsSubstituteRelayByDefault()
        {
            // Fixture setup
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Exercise system 
            var builder = sut.Builder;
            // Verify outcome
            Assert.IsType<SubstituteRelay>(builder);
            // Teardown
        }

        [Fact]
        public void CustomizeThrowsWhenFixtureIsNull()
        {
            // Fixture setup
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeInsertsSubstituteAttributeRelayInCustomizationsToOverrideDefaultConstructionWhenAttributeIsPresent()
        {
            // Fixture setup
            var sut = new AutoConfiguredNSubstituteCustomization();
            var fixture = Substitute.For<IFixture>();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            fixture.Customizations.Received().Insert(0, Arg.Any<SubstituteAttributeRelay>());
            // Teardown
        }

        [Fact]
        public void CustomizeAddsPostprocessorWithSubstituteRequestHandlerAndCommandsToCustomizations()
        {
            // Fixture setup
            var fixture = Substitute.For<IFixture>();
            Postprocessor postprocessor = null;
            fixture.Customizations.Insert(Arg.Any<int>(), Arg.Do<Postprocessor>(p => postprocessor = p));
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var substituteRequestHandler = Assert.IsAssignableFrom<SubstituteRequestHandler>(postprocessor.Builder);
            var substituteFactory = Assert.IsType<MethodInvoker>(substituteRequestHandler.SubstituteFactory);
            Assert.IsType<NSubstituteMethodQuery>(substituteFactory.Query);
            var compositeCommand = Assert.IsAssignableFrom<CompositeSpecimenCommand>(postprocessor.Command);
            Assert.True(compositeCommand.Commands.OfType<NSubstituteRegisterCallHandlerCommand>().Any());
            Assert.True(compositeCommand.Commands.OfType<NSubstituteSealedPropertiesCommand>().Any());
            // Teardown
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
