using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TypedBuilderComposerTest
    {
        [Fact]
        public void SutIsSpecimenBuilderComposer()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new TypedBuilderComposer(dummyType, dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderComposer>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TypedBuilderComposer(null, dummyBuilder));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFactoryThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TypedBuilderComposer(dummyType, null));
            // Teardown
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Fixture setup
            var expectedType = typeof(decimal);
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new TypedBuilderComposer(expectedType, dummyBuilder);
            // Exercise system
            Type result = sut.TargetType;
            // Verify outcome
            Assert.Equal(expectedType, result);
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            var dummyType = typeof(string);
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new TypedBuilderComposer(dummyType, expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var targetType = typeof(string);
            var factory = new DelegatingSpecimenBuilder();
            var sut = new TypedBuilderComposer(targetType, factory);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(result);

            var orSpec = Assert.IsAssignableFrom<OrRequestSpecification>(filter.Specification);
            Assert.Equal(targetType, orSpec.Specifications.OfType<SeedRequestSpecification>().Single().TargetType);
            Assert.Equal(targetType, orSpec.Specifications.OfType<ExactTypeSpecification>().Single().TargetType);

            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(filter.Builder);
#pragma warning disable 618
            var outputGuard = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(composite.Builders.First());
            Assert.IsAssignableFrom<SeedIgnoringRelay>(composite.Builders.Last());
#pragma warning restore 618

            Assert.Equal(factory, outputGuard.Builder);

            var inverseSpec = Assert.IsAssignableFrom<InverseRequestSpecification>(outputGuard.Specification);
            var seedSpec = Assert.IsAssignableFrom<SeedRequestSpecification>(inverseSpec.Specification);
            Assert.Equal(targetType, seedSpec.TargetType);
            // Teardown
        }
    }
}
