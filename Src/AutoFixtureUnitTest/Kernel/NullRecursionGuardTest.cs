using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
#pragma warning disable 618
    public class NullRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullRecursionGuard(new DelegatingSpecimenBuilder());
            // Verify outcome
            Assert.IsAssignableFrom<RecursionGuard>(sut);
            // Teardown
        }

        [Fact]
        public void ReturnsNullAtRecursionPoint()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new NullRecursionGuard(builder);
            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => sut.Create(r, container); // Provoke recursion

            // Exercise system
            object res = sut.Create(Guid.NewGuid(), container);

            Assert.Null(res);
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullRecursionGuard(dummyBuilder);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<NullRecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullRecursionGuard(dummyBuilder);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var rg = Assert.IsAssignableFrom<NullRecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
            // Teardown
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new NullRecursionGuard(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<NullRecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
            // Teardown
        }
    }
#pragma warning restore 618
}