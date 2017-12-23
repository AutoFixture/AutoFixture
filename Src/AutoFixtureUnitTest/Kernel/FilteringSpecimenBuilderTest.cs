using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class FilteringSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummySpecification = new DelegatingRequestSpecification();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderWillThrows()
        {
            // Arrange
            var dummySpecification = new DelegatingRequestSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new FilteringSpecimenBuilder(null, dummySpecification));
        }

        [Fact]
        public void InitializeWithNullSpecificationWillThrow()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new FilteringSpecimenBuilder(dummyBuilder, null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(expectedBuilder, dummySpecification);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void SpecificationIsCorrect()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expectedSpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, expectedSpecification);
            // Act
            IRequestSpecification result = sut.Specification;
            // Assert
            Assert.Equal(expectedSpecification, result);
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenSpecificationIsNotSatisfied()
        {
            // Arrange
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, spec);
            var request = new object();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void SpecificationReceivesCorrectRequest()
        {
            // Arrange
            var expectedRequest = new object();
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };

            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, specMock);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(expectedRequest, dummyContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenFilterAllowsRequestThrough()
        {
            // Arrange
            var expectedResult = new object();
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => true };
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var sut = new FilteringSpecimenBuilder(builder, spec);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreatePassesCorrectParametersToDecoratedBuilder()
        {
            // Arrange
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();
            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => true };
            var sut = new FilteringSpecimenBuilder(builderMock, spec);
            // Act
            sut.Create(expectedRequest, expectedContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void SutIsNode()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            // Act
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            // Act
            var sut = new FilteringSpecimenBuilder(expected, dummySpecification);
            // Assert
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(((System.Collections.IEnumerable)sut).Cast<object>()));
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var f = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(f.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var f = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(actual);
            Assert.Equal(expected, f.Builder);
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var f = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(actual);
            Assert.Equal(expected, f.Specification);
        }
    }
}
