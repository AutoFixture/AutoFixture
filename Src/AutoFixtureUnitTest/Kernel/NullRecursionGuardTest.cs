using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class NullRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Arrange
            // Act
            var sut = new NullRecursionGuard(new DelegatingSpecimenBuilder());
            // Assert
            Assert.IsAssignableFrom<RecursionGuard>(sut);
        }

        [Fact]
        public void ReturnsNullAtRecursionPoint()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new NullRecursionGuard(builder);
            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => sut.Create(r, container); // Provoke recursion

            // Act
            object res = sut.Create(Guid.NewGuid(), container);

            Assert.Null(res);
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullRecursionGuard(dummyBuilder);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var rg = Assert.IsAssignableFrom<NullRecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullRecursionGuard(dummyBuilder);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var rg = Assert.IsAssignableFrom<NullRecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new NullRecursionGuard(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var rg = Assert.IsAssignableFrom<NullRecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
        }
    }
}