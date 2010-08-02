using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class AutoMoqFixtureTest
    {
        [Fact]
        public void EnableNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                AutoMoqFixture.EnableAutoMocking(null));
            // Teardown
        }

        [Fact]
        public void EnableAutoMockingAddsAppropriateResidueCollector()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var composerStub = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
            composerStub.SetupGet(c => c.ResidueCollectors).Returns(residueCollectors);
            // Exercise system
            composerStub.Object.EnableAutoMocking();
            // Verify outcome
            Assert.True(residueCollectors.OfType<MockRelay>().Any());
            // Teardown
        }

        [Fact]
        public void EnableAutoMockingAddsAppropriateCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var composerStub = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
            composerStub.SetupGet(c => c.Customizations).Returns(customizations);
            // Exercise system
            composerStub.Object.EnableAutoMocking();
            // Verify outcome
            var postprocessor = customizations.OfType<MockPostprocessor>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<ConstructorInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<MockConstructorQuery>(ctorInvoker.Query);
            // Teardown
        }
    }
}
