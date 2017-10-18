using System;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CompositeSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeSpecimenBuilder();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void BuildersWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeSpecimenBuilder();
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenBuilder(null));
            // Teardown
        }

        [Fact]
        public void BuildersWillMatchListParameter()
        {
            // Fixture setup
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var sut = new CompositeSpecimenBuilder(expectedBuilders);
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.Equal(expectedBuilders, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullArrayWillThrow()
        {
            // Fixture setup
            ISpecimenBuilder[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenBuilder(nullArray));
            // Teardown
        }

        [Fact]
        public void BuildersWillMatchParamsArray()
        {
            // Fixture setup
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var sut = new CompositeSpecimenBuilder(expectedBuilders[0], expectedBuilders[1], expectedBuilders[2]);
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.Equal(expectedBuilders, result);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnFirstNonNoSpecimenResultFromBuilders()
        {
            // Fixture setup
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Exercise system
            var anonymousRequest = new object();
            var dummycontext = new DelegatingSpecimenContext();
            var result = sut.Create(anonymousRequest, dummycontext);
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnNullIfAllBuildersReturnNull()
        {
            // Fixture setup
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Exercise system
            var anonymousRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(anonymousRequest, dummyContext);
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
            {
                if (expectedRequest != r) throw new ArgumentException("Invalid context");
                mockVerified = true;
                return new object();
            };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            sut.Create(expectedRequest, dummyContext);
            // Verify outcome
            Assert.True(mockVerified);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Fixture setup
            var expectedContainer = new DelegatingSpecimenContext();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
            {
                if (expectedContainer != c) throw new ArgumentException("Invalid context");
                mockVerified = true;
                return new object();
            };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, expectedContainer);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
