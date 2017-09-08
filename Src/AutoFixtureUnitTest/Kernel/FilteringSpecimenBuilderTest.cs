using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FilteringSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummySpecification = new DelegatingRequestSpecification();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderWillThrows()
        {
            // Fixture setup
            var dummySpecification = new DelegatingRequestSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new FilteringSpecimenBuilder(null, dummySpecification));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new FilteringSpecimenBuilder(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(expectedBuilder, dummySpecification);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expectedSpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, expectedSpecification);
            // Exercise system
            IRequestSpecification result = sut.Specification;
            // Verify outcome
            Assert.Equal(expectedSpecification, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenSpecificationIsNotSatisfied()
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, spec);
            var request = new object();
            // Exercise system            
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void SpecificationReceivesCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };

            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, specMock);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(expectedRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenFilterAllowsRequestThrough()
        {
            // Fixture setup
            var expectedResult = new object();
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => true };
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var sut = new FilteringSpecimenBuilder(builder, spec);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreatePassesCorrectParametersToDecoratedBuilder()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();
            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => true };
            var sut = new FilteringSpecimenBuilder(builderMock, spec);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void SutIsNode()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            // Exercise system
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            // Exercise system
            var sut = new FilteringSpecimenBuilder(expected, dummySpecification);
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
            var dummySpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var f = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(f.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var f = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(actual);
            Assert.Equal(expected, f.Builder);
            // Teardown
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRequestSpecification();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var f = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(actual);
            Assert.Equal(expected, f.Specification);
            // Teardown
        }
    }
}
