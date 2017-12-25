using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class ListSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new ListSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        public void IsSatisfiedByNonEnumerableRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new ListSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<Version>))]
        public void IsSatisfiedByEnumerableRequestReturnsCorrectResult(Type request)
        {
            // Arrange
            var sut = new ListSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
