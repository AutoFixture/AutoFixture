using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class IntPtrGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new IntPtrGuard();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Foo")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        public void AnythingElseThanAnIntPtrRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new IntPtrGuard();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromIntPtrRequestThrows()
        {
            // Arrange
            var sut = new IntPtrGuard();
            var dummyContext = new DelegatingSpecimenContext();
            // Act & assert
            Assert.Throws<IllegalRequestException>(() =>
                sut.Create(typeof(IntPtr), dummyContext));
        }
    }
}
