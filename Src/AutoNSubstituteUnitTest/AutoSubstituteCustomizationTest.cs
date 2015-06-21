using System;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoSubstituteCustomizationTest
    {
        [Fact]
        public void SutIsICustomization()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(ICustomization).IsAssignableFrom(typeof(AutoSubstituteCustomization)));
            // Teardown
        }

        [Fact]
        public void CustomizeThrowsArgumentNullExceptionWhenFixtureIsNull()
        {
            // Fixture setup
            var sut = new AutoSubstituteCustomization();
            // Exercise system
            var e = Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
            // Verify outcome
            Assert.Equal("fixture", e.ParamName);
            // Teardown
        }

        [Fact]
        public void CustomizeInsertsSubstituteRelayInResidueCollectorsToSubstituteUnresolvedAbstractTypes()
        {
            // Fixture setup
            var sut = new AutoSubstituteCustomization();
            var fixture = Substitute.For<IFixture>();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            fixture.ResidueCollectors.Received().Insert(0, Arg.Any<SubstituteRelay>());
            // Teardown
        }

        [Fact]
        public void CustomizeInsertsSubstituteAttributeRelayInCustomizationsToOverrideDefaultConstructionWhenAttributeIsPresent()
        {
            // Fixture setup
            var sut = new AutoSubstituteCustomization();
            var fixture = Substitute.For<IFixture>();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            fixture.Customizations.Received().Insert(0, Arg.Any<SubstituteAttributeRelay>());
            // Teardown
        }

        [Fact]
        public void CustomizeInsertsProperlyConfiguredSubstituteBuilderInCustomizationsToHandleSubstituteRequests()
        {
            // Fixture setup
            var sut = new AutoSubstituteCustomization();
            SubstituteBuilder builder = null;
            var fixture = Substitute.For<IFixture>();
            fixture.Customizations.Insert(Arg.Any<int>(), Arg.Do<SubstituteBuilder>(b => builder = b));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.NotNull(builder);
            var substituteConstructor = Assert.IsType<MethodInvoker>(builder.SubstituteConstructor);
            Assert.IsType<NSubstituteMethodQuery>(substituteConstructor.Query);
            // Teardown
        }
    }
}
