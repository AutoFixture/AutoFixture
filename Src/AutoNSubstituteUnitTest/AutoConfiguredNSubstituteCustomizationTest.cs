using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoConfiguredNSubstituteCustomizationTest
    {
        [Fact]
        public void CtorThrowsWhenRelayIsNull()
        {
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoConfiguredNSubstituteCustomization(null));
        }

        [Fact]
        public void BuilderIsNSubstituteBuilderByDefault()
        {
            // Fixture setup
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Exercise system 
            var builder = sut.Builder;
            // Verify outcome
            Assert.IsType<NSubstituteBuilder>(builder);
            // Teardown
        }

        [Fact]
        public void CustomizeThrowsWhenFixtureIsNull()
        {
            // Fixture setup
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsNSubstitutePostprocessorCommandsToResidueCollectors()
        {
            // Fixture setup
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixture = Substitute.For<IFixture>();
            fixture.ResidueCollectors.Returns(residueCollectors);
            var sut = new AutoConfiguredNSubstituteCustomization();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var postprocessor =
                Assert.Single(residueCollectors.OfType<Postprocessor>());
            var nsubstituteBuilder = Assert.IsAssignableFrom<NSubstituteBuilder>(postprocessor.Builder);
            var methodInvoker = Assert.IsAssignableFrom<MethodInvoker>(nsubstituteBuilder.Builder);
            Assert.IsAssignableFrom<AbstractTypeSpecification>(nsubstituteBuilder.SubstitutionSpecification);
            Assert.IsAssignableFrom<NSubstituteMethodQuery>(methodInvoker.Query);
            var compositeCommand = Assert.IsAssignableFrom<CompositeSpecimenCommand>(postprocessor.Command);
            Assert.True(compositeCommand.Commands.OfType<NSubstituteVirtualMethodsCommand>().Any());
            Assert.True(compositeCommand.Commands.OfType<NSubstituteSealedPropertiesCommand>().Any());
            // Teardown
        }

        [Fact]
        public void CustomizeAddsBuilderToResidueCollectors()
        {
            var builder = Substitute.For<ISpecimenBuilder>();
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixture = Substitute.For<IFixture>();
            fixture.ResidueCollectors.Returns(residueCollectors);
            var sut = new AutoConfiguredNSubstituteCustomization(builder);

            sut.Customize(fixture);

            var postprocessor =
                Assert.Single(residueCollectors.OfType<Postprocessor>());
            Assert.Equal(builder, postprocessor.Builder);
        }

        [Fact]
        public void CustomizeAddsEnumeratorRelayToResidueCollectors()
        {
            var builder = Substitute.For<ISpecimenBuilder>();
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixture = Substitute.For<IFixture>();
            fixture.ResidueCollectors.Returns(residueCollectors);
            var sut = new AutoConfiguredNSubstituteCustomization(builder);

            sut.Customize(fixture);

            Assert.Single(residueCollectors.OfType<EnumeratorRelay>());
        }

        [Fact]
        public void CustomizeAddsEnumeratorRelayInCorrectOrder()
        {
            var builder = Substitute.For<ISpecimenBuilder>();
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixture = Substitute.For<IFixture>();
            fixture.ResidueCollectors.Returns(residueCollectors);
            var sut = new AutoConfiguredNSubstituteCustomization(builder);

            sut.Customize(fixture);

            Assert.IsAssignableFrom<EnumeratorRelay>(residueCollectors.First());
        }
    }
}
