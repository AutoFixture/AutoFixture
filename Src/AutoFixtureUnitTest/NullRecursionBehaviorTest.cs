using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class NullRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Arrange
            // Act
            var sut = new NullRecursionBehavior();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Arrange
            var sut = new NullRecursionBehavior();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
        }

        [Fact]
        public void TransformReturnsCorrectResultForDefaultRecursionDepth()
        {
            // Arrange
            var sut = new NullRecursionBehavior();
            // Act
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.IsAssignableFrom<NullRecursionHandler>(rg.RecursionHandler);
            Assert.Equal(1, rg.RecursionDepth);
        }

        [Fact]
        public void TransformReturnsCorrectResultForSpecificRecursionDepth()
        {
            // Arrange
            const int explicitRecursionDepth = 2;
            var sut = new NullRecursionBehavior(explicitRecursionDepth);
            // Act
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.IsAssignableFrom<NullRecursionHandler>(rg.RecursionHandler);
            Assert.Equal(explicitRecursionDepth, rg.RecursionDepth);
        }

        [Fact]
        public void TransformResultCorrectlyDecoratesInput()
        {
            // Arrange
            var sut = new NullRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Act
            var result = sut.Transform(expectedBuilder);
            // Assert
            var guard = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
        }
    }
}
