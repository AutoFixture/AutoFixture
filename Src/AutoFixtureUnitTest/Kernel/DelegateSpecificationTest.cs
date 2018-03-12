namespace AutoFixtureUnitTest.Kernel
{
    using System;
    using AutoFixture.Kernel;
    using Xunit;

    public class DelegateSpecificationTest
    {
        public delegate string CustomDelegate(object o);

        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new DelegateSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("a string", false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(Func<int>), true)]
        [InlineData(typeof(Action<string, object>), true)]
        [InlineData(typeof(CustomDelegate), true)]
        public void IsSatisfiedByReturnsCorrectResult(object request, bool expectedResult)
        {
            // Arrange
            var sut = new DelegateSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}