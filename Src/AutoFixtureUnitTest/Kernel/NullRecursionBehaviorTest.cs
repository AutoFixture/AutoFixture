using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NullRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullRecursionBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Fixture setup
            var sut = new NullRecursionBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullRecursionBehavior();
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<NullRecursionGuard>(result);
            // Teardown
        }

        [Fact]
        public void TranformResultCorrectlyDecoratesInput()
        {
            // Fixture setup
            var sut = new NullRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(expectedBuilder);
            // Verify outcome
            var guard = Assert.IsAssignableFrom<NullRecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
            // Teardown
        }
    }
}
