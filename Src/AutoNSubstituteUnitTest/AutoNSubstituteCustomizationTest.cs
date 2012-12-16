using System;
using System.Collections.Generic;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
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
        public void BuilderIsNSubstituteBuilder_WhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            Assert.IsType<NSubstituteBuilder>(result);
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
        public void CustomizeAddsNoCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = Substitute.For<IFixture>();
            fixtureStub.Customizations.Returns(customizations);

            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            sut.Customize(fixtureStub);
            // Verify outcome
            Assert.Empty(customizations);
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