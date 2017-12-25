using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class OmitOnRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Arrange
            // Act
            var sut = new OmitOnRecursionBehavior();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Arrange
            var sut = new OmitOnRecursionBehavior();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
        }

        [Fact]
        public void TransformReturnsCorrectResultForDefaultRecursionDepth()
        {
            // Arrange
            var sut = new OmitOnRecursionBehavior();
            // Act
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.IsAssignableFrom<OmitOnRecursionHandler>(rg.RecursionHandler);
            Assert.Equal(1, rg.RecursionDepth);
        }

        [Fact]
        public void TransformReturnsCorrectResultForSpecificRecursionDepth()
        {
            // Arrange
            const int explicitRecursionDepth = 2;
            var sut = new OmitOnRecursionBehavior(explicitRecursionDepth);
            // Act
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.IsAssignableFrom<OmitOnRecursionHandler>(rg.RecursionHandler);
            Assert.Equal(explicitRecursionDepth, rg.RecursionDepth);
        }

        [Fact]
        public void TransformResultCorrectlyDecoratesInput()
        {
            // Arrange
            var sut = new OmitOnRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Act
            var result = sut.Transform(expectedBuilder);
            // Assert
            var guard = Assert.IsAssignableFrom<RecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        [InlineData(-42)]
        public void ConstructorWithRecursionDepthLowerThanOneThrows(int recursionDepth)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new OmitOnRecursionBehavior(recursionDepth));
        }
    }
}
