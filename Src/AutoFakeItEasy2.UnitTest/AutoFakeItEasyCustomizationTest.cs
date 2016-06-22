using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class AutoFakeItEasyCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoFakeItEasyCustomization();
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
                new AutoFakeItEasyCustomization(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithRelay()
        {
            // Fixture setup
            var expectedRelay = new FakeItEasyRelay();
            var sut = new AutoFakeItEasyCustomization(expectedRelay);
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
            var sut = new AutoFakeItEasyCustomization();
            // Exercise system
            var result = sut.Relay;
            // Verify outcome
            Assert.IsType<FakeItEasyRelay>(result);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new AutoFakeItEasyCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = new Fake<IFixture>();
            fixtureStub.CallsTo(c => c.Customizations).Returns(customizations);

            var sut = new AutoFakeItEasyCustomization();
            // Exercise system
            sut.Customize(fixtureStub.FakedObject);
            // Verify outcome
            var postprocessor = customizations.OfType<FakeItEasyBuilder>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<FakeItEasyMethodQuery>(ctorInvoker.Query);
            // Teardown
        }

        [Fact]
        public void CustomizeAddsAppropriateResidueCollector()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = new Fake<IFixture>();
            fixtureStub.CallsTo(c => c.ResidueCollectors).Returns(residueCollectors);
            
            var sut = new AutoFakeItEasyCustomization();
            // Exercise system
            sut.Customize(fixtureStub.FakedObject);
            // Verify outcome
            Assert.Contains(sut.Relay, residueCollectors);
            // Teardown
        }
    }
}