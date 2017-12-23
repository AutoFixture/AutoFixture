using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class CompositeSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new CompositeSpecimenBuilder();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void SutIsSequenceOfSpecimenBuilders()
        {
            // Arrange
            // Act
            var sut = new CompositeSpecimenBuilder();
            // Assert
            Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
        }

        [Fact]
        public void SutIsNode()
        {
            // Arrange
            // Act
            var sut = new CompositeSpecimenBuilder();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void BuildersWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new CompositeSpecimenBuilder();
            // Act
            IEnumerable<ISpecimenBuilder> result = sut.Builders;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Arrange
            IEnumerable<ISpecimenBuilder> nullEnumerable = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeSpecimenBuilder(nullEnumerable));
        }

        [Fact]
        public void BuildersWillMatchListParameter()
        {
            // Arrange
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            }.AsEnumerable();
            var sut = new CompositeSpecimenBuilder(expectedBuilders);
            // Act
            var result = sut.Builders;
            // Assert
            Assert.True(expectedBuilders.SequenceEqual(result), "Builders");
        }

        [Fact]
        public void SutYieldsInjectedSequence()
        {
            // Arrange
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            }.AsEnumerable();
            var sut = new CompositeSpecimenBuilder(expectedBuilders);
            // Act
            // Assert
            Assert.True(expectedBuilders.SequenceEqual(sut));
            Assert.True(expectedBuilders.OfType<object>().SequenceEqual(
                ((System.Collections.IEnumerable)sut).OfType<object>()));
        }

        [Fact]
        public void CreateWithNullArrayWillThrow()
        {
            // Arrange
            ISpecimenBuilder[] nullArray = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeSpecimenBuilder(nullArray));
        }

        [Fact]
        public void BuildersWillMatchParamsArray()
        {
            // Arrange
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var sut = new CompositeSpecimenBuilder(expectedBuilders[0], expectedBuilders[1], expectedBuilders[2]);
            // Act
            var result = sut.Builders;
            // Assert
            Assert.True(expectedBuilders.SequenceEqual(result), "Builders");
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var sut = new CompositeSpecimenBuilder();
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(actual);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void CreateWillReturnFirstSpecimenResultFromBuilders()
        {
            // Arrange
            var expectedResult = new object();
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Act
            var anonymousRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(anonymousRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWillReturnNoSpecimenIfAllBuildersReturnNoSpecimen()
        {
            // Arrange
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Act
            var anonymousRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(anonymousRequest, dummyContainer);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Arrange
            var expectedRequest = new object();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.Equal(expectedRequest, r);
                    mockVerified = true;
                    return new object();
                };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(expectedRequest, dummyContainer);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Arrange
            var expectedContainer = new DelegatingSpecimenContext();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.Equal(expectedContainer, c);
                    mockVerified = true;
                    return new object();
                };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, expectedContainer);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
