using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class AutoRhinoMockCustomizationTest
    {
        [Fact]
        public void SutImplementsICustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoRhinoMockCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new AutoRhinoMockCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullRelayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoRhinoMockCustomization((ISpecimenBuilder)null));
            // Teardown
        }

        [Fact]
        public void RelayIsCorrect()
        {
            // Fixture setup
            var expected = MockRepository.GenerateMock<ISpecimenBuilder>();

            // Exercise system
            var sut = new AutoRhinoMockCustomization(expected);
            // Verify outcome
            Assert.Equal(expected, sut.MockRelay);
            // Teardown
        }

        [Fact]
        public void CustomizeAddsAppropriateCustomizations()
        {
            // Fixture setup
            var customizations = new List<ISpecimenBuilder>();
            var fixtureStub = MockRepository.GenerateMock<IFixture>();
            fixtureStub.Stub(c => c.Customizations).Return(customizations);
            fixtureStub.Stub(fixture => fixture.ResidueCollectors).Return(new List<ISpecimenBuilder>());

            var sut = new AutoRhinoMockCustomization();
            // Exercise system
            sut.Customize(fixtureStub);
            // Verify outcome
            var postprocessor = customizations.OfType<RhinoMockPostprocessor>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<ConstructorInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<RhinoMockConstructorQuery>(ctorInvoker.Query);
            // Teardown
        }
    }
}
