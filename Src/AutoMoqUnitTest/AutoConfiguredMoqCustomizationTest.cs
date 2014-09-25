using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class AutoConfiguredMoqCustomizationTest
    {
        [Fact]
        public void CtorThrowsWhenRelayIsNull()
        {
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoConfiguredMoqCustomization(null));
        }

        [Fact]
        public void RelayIsMockRelayByDefault()
        {
            // Fixture setup
            var sut = new AutoConfiguredMoqCustomization();
            // Exercise system 
            var relay = sut.Relay;
            // Verify outcome
            Assert.IsType<MockRelay>(relay);
            // Teardown
        }

        [Fact]
        public void CustomizeThrowsWhenFixtureIsNull()
        {
            // Fixture setup
            var sut = new AutoConfiguredMoqCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsPostprocessorToCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixture = new Mock<IFixture> {DefaultValue = DefaultValue.Mock};
            fixture.Setup(f => f.Customizations)
                   .Returns(customizations);

            var sut = new AutoConfiguredMoqCustomization();
            // Exercise system
            sut.Customize(fixture.Object);
            // Verify outcome
            Assert.True(customizations.Any(builder => builder is Postprocessor));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsMockCommandsToPostprocessor()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixture = new Mock<IFixture> {DefaultValue = DefaultValue.Mock};
            fixture.Setup(f => f.Customizations)
                   .Returns(customizations);

            var sut = new AutoConfiguredMoqCustomization();
            // Exercise system
            sut.Customize(fixture.Object);
            // Verify outcome
            var postprocessor = (Postprocessor) customizations.Single(builder => builder is Postprocessor);
            var compositeCommand = (CompositeSpecimenCommand) postprocessor.Command;

            Assert.True(compositeCommand.Commands.Any(init => init is MockVirtualMethodsCommand));
            Assert.True(compositeCommand.Commands.Any(init => init is MockSealedPropertiesCommand));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsRelayToResidueCollectors()
        {
            // Fixture setup
            var relay = new Mock<ISpecimenBuilder>();
            var collectors = new List<ISpecimenBuilder>();
            var fixture = new Mock<IFixture> {DefaultValue = DefaultValue.Mock};
            fixture.Setup(f => f.ResidueCollectors)
                   .Returns(collectors);

            var sut = new AutoConfiguredMoqCustomization(relay.Object);
            // Exercise system
            sut.Customize(fixture.Object);
            // Verify outcome
            Assert.Contains(relay.Object, collectors);
            // Teardown
        }
    }
}
