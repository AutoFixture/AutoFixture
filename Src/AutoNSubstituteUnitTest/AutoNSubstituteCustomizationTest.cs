using System;
using System.Collections.Generic;
using System.Linq;
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
        public void InitializeWithNullRelayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoNSubstituteCustomization(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithRelay()
        {
            // Fixture setup
            var expectedRelay = new NSubstituteRelay();
            var sut = new AutoNSubstituteCustomization(expectedRelay);
            // Exercise system
            ISpecimenBuilder result = sut.Relay;
            // Verify outcome
            Assert.Equal(expectedRelay, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            var result = sut.Relay;
            // Verify outcome
            Assert.IsType<NSubstituteRelay>(result);
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
            Assert.Contains(sut.Relay, residueCollectors);
            // Teardown
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = Substitute.For<IFixture>();
            fixtureStub.Customizations.Returns(customizations);

            var sut = new AutoNSubstituteCustomization();
            // Exercise system
            sut.Customize(fixtureStub);
            // Verify outcome
            var postprocessor = customizations.OfType<NSubstituteBuilder>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<NSubstituteConstructorQuery>(ctorInvoker.Query);
            // Teardown
        }
    }
}
