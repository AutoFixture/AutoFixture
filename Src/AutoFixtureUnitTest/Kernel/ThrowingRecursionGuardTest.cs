using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class ThrowingRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Arrange
            // Act
            var sut = new ThrowingRecursionGuard(new DelegatingSpecimenBuilder());
            // Assert
            Assert.IsAssignableFrom<RecursionGuard>(sut);
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new ThrowingRecursionGuard(expectedBuilder);
            // Act
            var result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void ThrowsAtRecursionPoint()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new ThrowingRecursionGuard(builder);
            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => sut.Create(r, container); // Provoke recursion

            // Act
            Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(Guid.NewGuid(), container));
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new ThrowingRecursionGuard(dummyBuilder);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var rg = Assert.IsAssignableFrom<ThrowingRecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new ThrowingRecursionGuard(dummyBuilder);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var rg = Assert.IsAssignableFrom<ThrowingRecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new ThrowingRecursionGuard(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var rg = Assert.IsAssignableFrom<ThrowingRecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
        }
    }
}