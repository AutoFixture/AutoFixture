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
            // Arrange
            // Act
            var sut = new AutoNSubstituteCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoNSubstituteCustomization(null));
        }

        [Fact]
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

        [Fact]
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
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new AutoNSubstituteCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
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
            SubstituteRequestHandler builder = null;
            var fixture = Substitute.For<IFixture>();
            fixture.Customizations.Insert(Arg.Any<int>(), Arg.Do<SubstituteRequestHandler>(b => builder = b));
            // Act
            sut.Customize(fixture);
            // Assert
            Assert.NotNull(builder);
            var substituteConstructor = Assert.IsType<MethodInvoker>(builder.SubstituteFactory);
            Assert.IsType<NSubstituteMethodQuery>(substituteConstructor.Query);
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
            Assert.Contains(sut.Builder, residueCollectors);
        }
    }
}