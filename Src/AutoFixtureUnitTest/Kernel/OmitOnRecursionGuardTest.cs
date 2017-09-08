using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
#pragma warning disable 618
    public class OmitOnRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Fixture setup
            // Exercise system
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummy);
            // Verify outcome
            Assert.IsAssignableFrom<RecursionGuard>(sut);
            // Teardown
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummyBuilder);
            // Exercise system
            var dummyRequest = new object();
            var actual = sut.HandleRecursiveRequest(dummyRequest);
            // Verify outcome
            var expected = new OmitSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructorThrowsOnNullComparer()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new OmitOnRecursionGuard(dummy, null));
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummyBuilder);

            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            // Exercise system
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<OmitOnRecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitOnRecursionGuard(dummyBuilder);

            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var rg = Assert.IsAssignableFrom<OmitOnRecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
            // Teardown
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new OmitOnRecursionGuard(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<OmitOnRecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
            // Teardown
        }
    }
#pragma warning restore 618
}
