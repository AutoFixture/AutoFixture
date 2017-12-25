using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenOutputGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeModestCtorWithNullBuilderThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null));
        }

        [Fact]
        public void InitializeGreedyCtorWithNullBuilderThrows()
        {
            // Arrange
            var dummySpec = new DelegatingRequestSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null, dummySpec));
        }

        [Fact]
        public void InitializeGreedyCtorWithNullSpecificationThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(dummyBuilder, null));
        }

        [Fact]
        public void BuilderIsCorrectWhenInitializedWithModestCtor()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(expectedBuilder);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void BuilderIsCorrectWhenInitializedWithGreedyCtor()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var dummySpec = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(expectedBuilder, dummySpec);
            // Act
            var result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithModestCtor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Act
            IRequestSpecification result = sut.Specification;
            // Assert
            Assert.IsAssignableFrom<TrueRequestSpecification>(result);
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithGreedyCtor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expectedSpec = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(dummyBuilder, expectedSpec);
            // Act
            var result = sut.Specification;
            // Assert
            Assert.Equal(expectedSpec, result);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request && c == context ? expectedResult : new NoSpecimen() };

            var sut = new NoSpecimenOutputGuard(builder);
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateThrowsWhenDecoratedBuilderReturnsNoSpecimen()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() };
            var sut = new NoSpecimenOutputGuard(builder);
            // Act & assert
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create(dummyRequest, dummyContext));
        }

        [Fact]
        public void CreateDoesNotThrowOnReturnedNoSpecimenWhenSpecificationReturnsFalse()
        {
            // Arrange
            var request = new object();

            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() };
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => request == r ? false : true };
            var sut = new NoSpecimenOutputGuard(builder, spec);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void SutIsNode()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            // Act
            var sut = new NoSpecimenOutputGuard(expected);
            // Assert
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(((System.Collections.IEnumerable)sut).Cast<object>()));
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var g = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(g.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var g = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(actual);
            Assert.Equal(expected, g.Builder);
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var g = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(actual);
            Assert.Equal(expected, g.Specification);
        }
    }
}
