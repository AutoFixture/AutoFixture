using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class AutoMoqCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoMoqCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new AutoMoqCustomization();
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
            var fixtureStub = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
            fixtureStub.SetupGet(c => c.ResidueCollectors).Returns(residueCollectors);

            var sut = new AutoMoqCustomization();
            // Exercise system
            sut.Customize(fixtureStub.Object);
            // Verify outcome
            Assert.True(residueCollectors.OfType<MockRelay>().Any());
            // Teardown
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
            fixtureStub.SetupGet(c => c.Customizations).Returns(customizations);

            var sut = new AutoMoqCustomization();
            // Exercise system
            sut.Customize(fixtureStub.Object);
            // Verify outcome
            var postprocessor = customizations.OfType<MockPostprocessor>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<ConstructorInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<MockConstructorQuery>(ctorInvoker.Query);
            // Teardown
        }
    }
}
