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
        public void SutImplementsICustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoFakeItEasyCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
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
        public void CustomizeAddsAppropriateResidueCollectors()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = new Fake<IFixture>();
            fixtureStub.CallsTo(c => c.ResidueCollectors).Returns(residueCollectors);

            var sut = new AutoFakeItEasyCustomization();
            // Exercise system
            sut.Customize(fixtureStub.FakedObject);
            // Verify outcome
            var postProcessor = residueCollectors.OfType<FakeItEasyBuilder>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postProcessor.Builder);
            Assert.IsAssignableFrom<FakeItEasyMethodQuery>(ctorInvoker.Query);
            // Teardown
        }
    }
}
