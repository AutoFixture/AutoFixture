using System;
using System.Collections.Generic;
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
            // Fixture setup
            // Exercise system
            var sut = new AutoNSubstituteCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoNSubstituteCustomization(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithBuilder()
        {
            // Fixture setup
            var expectedBuilder = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>());
            var sut = new AutoNSubstituteCustomization(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void BuilderIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void BuilderIsSubstituteRelay_WhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            Assert.IsType<SubstituteRelay>(result);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeInsertsSubstituteAttributeRelayInCustomizationsToOverrideDefaultConstructionWhenAttributeIsPresent()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            var fixture = Substitute.For<IFixture>();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            fixture.Customizations.Received().Insert(0, Arg.Any<SubstituteAttributeRelay>());
            // Teardown
        }

        [Fact]
        public void CustomizeInsertsProperlyConfiguredSubstituteRequestHandlerInCustomizationsToHandleSubstituteRequests()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            SubstituteRequestHandler builder = null;
            var fixture = Substitute.For<IFixture>();
            fixture.Customizations.Insert(Arg.Any<int>(), Arg.Do<SubstituteRequestHandler>(b => builder = b));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.NotNull(builder);
            var substituteConstructor = Assert.IsType<MethodInvoker>(builder.SubstituteFactory);
            Assert.IsType<NSubstituteMethodQuery>(substituteConstructor.Query);
            // Teardown
        }

        [Fact]
        public void CustomizeAddsAppropriateResidueCollector()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = Substitute.For<IFixture>();
            fixtureStub.ResidueCollectors.Returns(residueCollectors);
            
            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            sut.Customize(fixtureStub);
            // Verify outcome
            Assert.Contains(sut.Builder, residueCollectors);
            // Teardown
        }
    }
}