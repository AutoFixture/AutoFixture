using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class HashSetSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new HashSetSpecification();
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
        public void IsSatisfiedByNonHashSetRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new HashSetSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(HashSet<object>))]
        [InlineData(typeof(HashSet<string>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(HashSet<Version>))]
        public void IsSatisfiedByCollectionRequestReturnsCorrectResult(Type request)
        {
            // Arrange
            var sut = new HashSetSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
