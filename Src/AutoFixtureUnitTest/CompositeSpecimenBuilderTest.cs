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
            // Arrange
            // Act
            var sut = new CompositeSpecimenBuilder();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void BuildersWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new CompositeSpecimenBuilder();
            // Act
            var result = sut.Builders;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenBuilder(null));
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
            };
            var sut = new CompositeSpecimenBuilder(expectedBuilders);
            // Act
            var result = sut.Builders;
            // Assert
            Assert.Equal(expectedBuilders, result);
        }

        [Fact]
        public void CreateWithNullArrayWillThrow()
        {
            // Arrange
            ISpecimenBuilder[] nullArray = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenBuilder(nullArray));
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
            Assert.Equal(expectedBuilders, result);
        }

        [Fact]
        public void CreateWillReturnFirstNonNoSpecimenResultFromBuilders()
        {
            // Arrange
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Act
            var anonymousRequest = new object();
            var dummycontext = new DelegatingSpecimenContext();
            var result = sut.Create(anonymousRequest, dummycontext);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateWillReturnNullIfAllBuildersReturnNull()
        {
            // Arrange
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Act
            var anonymousRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(anonymousRequest, dummyContext);
            // Assert
            Assert.Null(result);
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
                if (expectedRequest != r) throw new ArgumentException("Invalid context");
                mockVerified = true;
                return new object();
            };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            sut.Create(expectedRequest, dummyContext);
            // Assert
            Assert.True(mockVerified);
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
                if (expectedContainer != c) throw new ArgumentException("Invalid context");
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
