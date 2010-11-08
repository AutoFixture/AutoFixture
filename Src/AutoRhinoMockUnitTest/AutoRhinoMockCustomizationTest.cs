using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace AutoRhinoMockUnitTest
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
        public void RelayIsCorrectWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new AutoRhinoMockCustomization();
            // Exercise system
            var result = sut.MockRelay;
            // Verify outcome
            Assert.IsType<RhinoMockRelay>(result);
            // Teardown
        }

        [Fact]
        public void GenericEnumerableBuildersIsCorrectWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new AutoRhinoMockCustomization();
            var expected = new[] {typeof (DefaultGenericListBuilder), typeof (DefaultGenericDictionaryBuilder)}.ToList();

            // Exercise system
            var result = (from ge in sut.GenericEnumerableBuilders select ge.GetType()).ToList();
            // Verify outcome
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullRelayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoRhinoMockCustomization((ISpecimenBuilder)null, MockRepository.GenerateMock<ISpecimenBuilder>()));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullGenericEnumerableBuildersThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoRhinoMockCustomization(MockRepository.GenerateMock<ISpecimenBuilder>(), (ISpecimenBuilder)null));
            // Teardown
        }

        [Fact]
        public void RelayIsCorrect()
        {
            // Fixture setup
            var expected = MockRepository.GenerateMock<ISpecimenBuilder>();

            // Exercise system
            var sut = new AutoRhinoMockCustomization(expected, MockRepository.GenerateMock<ISpecimenBuilder>());
            // Verify outcome
            Assert.Equal(expected, sut.MockRelay);
            // Teardown
        }

        [Fact]
        public void GenericEnumerableBuildersIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = fixture.Repeat(() => MockRepository.GenerateMock<ISpecimenBuilder>()).ToList();

            // Exercise system
            var sut = new AutoRhinoMockCustomization(MockRepository.GenerateMock<ISpecimenBuilder>(), expected.ToArray());
            // Verify outcome
            Assert.True(expected.SequenceEqual(sut.GenericEnumerableBuilders));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsRelayToResidueCollector()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = MockRepository.GenerateMock<IFixture>();
            fixtureStub.Stub(fixture => fixture.Customizations).Return(new List<ISpecimenBuilder>());
            fixtureStub.Stub(fixture => fixture.ResidueCollectors).Return(residueCollectors);

            var sut = new AutoRhinoMockCustomization();
            // Exercise system
            sut.Customize(fixtureStub);
            // Verify outcome
            Assert.Contains(sut.MockRelay, residueCollectors);
            // Teardown
        }

        [Fact]
        public void CustomizeAddsEnumerableBuildersToResidueCollector()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = MockRepository.GenerateMock<IFixture>();
            fixtureStub.Stub(fixture => fixture.Customizations).Return(new List<ISpecimenBuilder>());
            fixtureStub.Stub(fixture => fixture.ResidueCollectors).Return(residueCollectors);

            var sut = new AutoRhinoMockCustomization();
            // Exercise system
            sut.Customize(fixtureStub);
            // Verify outcome
            sut.GenericEnumerableBuilders.ToList().ForEach(eb => 
                Assert.Contains(eb, residueCollectors));
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
