using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class OmitOnRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new OmitOnRecursionBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Fixture setup
            var sut = new OmitOnRecursionBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResultForDefaultRecursionDepth()
        {
            // Fixture setup
            var sut = new OmitOnRecursionBehavior();
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.IsAssignableFrom<OmitOnRecursionHandler>(rg.RecursionHandler);
            Assert.Equal(1, rg.RecursionDepth);
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResultForSpecificRecursionDepth()
        {
            // Fixture setup
            const int explicitRecursionDepth = 2;
            var sut = new OmitOnRecursionBehavior(explicitRecursionDepth);
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.IsAssignableFrom<OmitOnRecursionHandler>(rg.RecursionHandler);
            Assert.Equal(explicitRecursionDepth, rg.RecursionDepth);
            // Teardown
        }

        [Fact]
        public void TransformResultCorrectlyDecoratesInput()
        {
            // Fixture setup
            var sut = new OmitOnRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(expectedBuilder);
            // Verify outcome
            var guard = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        [InlineData(-42)]
        public void ConstructorWithRecursionDepthLowerThanOneThrows(int recursionDepth)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new OmitOnRecursionBehavior(recursionDepth));
            // Teardown
        }
    }
}
