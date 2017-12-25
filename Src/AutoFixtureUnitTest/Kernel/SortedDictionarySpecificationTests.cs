using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class SortedDictionarySpecificationTests
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new SortedDictionarySpecification();
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
        [InlineData(typeof(int?))]
        [InlineData(typeof(EmptyEnum?))]
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        [InlineData(typeof(int?[]))]
        [InlineData(typeof(EmptyEnum?[]))]
        public void IsSatisfiedByNonSortedSetRequestReturnsCorrectResult(object request)
        {
            // Arrange.
            var sut = new SortedDictionarySpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(SortedDictionary<string, int>))]
        [InlineData(typeof(SortedDictionary<int, string>))]
        [InlineData(typeof(SortedDictionary<object, object>))]
        [InlineData(typeof(SortedDictionary<Version, ConcreteType>))]
        public void IsSatisfiedBySortedSetRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new SortedDictionarySpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
