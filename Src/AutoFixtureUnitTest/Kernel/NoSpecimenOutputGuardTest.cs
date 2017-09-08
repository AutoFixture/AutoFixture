using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenOutputGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeModestCtorWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null));
            // Teardown
        }

        [Fact]
        public void InitializeGreedyCtorWithNullBuilderThrows()
        {
            // Fixture setup
            var dummySpec = new DelegatingRequestSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null, dummySpec));
            // Teardown
        }

        [Fact]
        public void InitializeGreedyCtorWithNullSpecificationThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrectWhenInitializedWithModestCtor()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrectWhenInitializedWithGreedyCtor()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var dummySpec = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(expectedBuilder, dummySpec);
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithModestCtor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Exercise system
            IRequestSpecification result = sut.Specification;
            // Verify outcome
            Assert.IsAssignableFrom<TrueRequestSpecification>(result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithGreedyCtor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expectedSpec = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(dummyBuilder, expectedSpec);
            // Exercise system
            var result = sut.Specification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request && c == context ? expectedResult : new NoSpecimen() };

            var sut = new NoSpecimenOutputGuard(builder);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenDecoratedBuilderReturnsNoSpecimen()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() };
            var sut = new NoSpecimenOutputGuard(builder);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create(dummyRequest, dummyContext));
            // Teardown
        }

        [Fact]
        public void CreateDoesNotThrowOnReturnedNoSpecimenWhenSpecificationReturnsFalse()
        {
            // Fixture setup            
            var request = new object();
            
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() };
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => request == r ? false : true };
            var sut = new NoSpecimenOutputGuard(builder, spec);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void SutIsNode()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new NoSpecimenOutputGuard(expected);
            // Verify outcome
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(((System.Collections.IEnumerable)sut).Cast<object>()));
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var g = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(g.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var g = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(actual);
            Assert.Equal(expected, g.Builder);
            // Teardown
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var g = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(actual);
            Assert.Equal(expected, g.Specification);
            // Teardown
        }
    }
}
