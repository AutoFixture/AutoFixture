using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class OmitOnRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Arrange
            // Act
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummy);
            // Assert
            Assert.IsAssignableFrom<RecursionGuard>(sut);
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummyBuilder);
            // Act
            var dummyRequest = new object();
            var actual = sut.HandleRecursiveRequest(dummyRequest);
            // Assert
            var expected = new OmitSpecimen();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructorThrowsOnNullComparer()
        {
            // Arrange
            var dummy = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new OmitOnRecursionGuard(dummy, null));
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummyBuilder);

            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            // Act
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var rg = Assert.IsAssignableFrom<OmitOnRecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummyBuilder);

            var expected = new DelegatingSpecimenBuilder();
            // Act
            var actual = sut.Compose(new[] { expected });
            // Assert
            var rg = Assert.IsAssignableFrom<OmitOnRecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new OmitOnRecursionGuard(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var rg = Assert.IsAssignableFrom<OmitOnRecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
        }
    }
}
