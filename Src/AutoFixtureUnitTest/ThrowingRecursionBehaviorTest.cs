using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ThrowingRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new ThrowingRecursionBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ThrowingRecursionGuard>(result);
            // Teardown
        }

        [Fact]
        public void TranformResultCorrectlyDecoratesInput()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(expectedBuilder);
            // Verify outcome
            var guard = Assert.IsAssignableFrom<ThrowingRecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
            // Teardown
        }

        [Fact]
        public void SutIsPipe()
        {
            // Fixture setup            
            // Exercise system
            var sut = new ThrowingRecursionBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderPipe>(sut);
            // Teardown
        }

        [Fact]
        public void PipeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            var builders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            // Exercise system
            var result = sut.Pipe(builders);
            // Verify outcome
            var guard = Assert.IsAssignableFrom<ThrowingRecursionGuard>(result.Single());
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(guard.Builder);
            Assert.True(builders.SequenceEqual(composite.Builders));
            // Teardown
        }

        [Fact]
        public void PipeNullThrows()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Pipe(null).ToList());
            // Teardown
        }
    }
}
