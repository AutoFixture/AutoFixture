using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class SortedSetSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new SortedSetSpecification();
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
            var sut = new SortedSetSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(SortedSet<string>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(SortedSet<object>))]
        [InlineData(typeof(SortedSet<Version>))]
        [InlineData(typeof(SortedSet<int?>))]
        [InlineData(typeof(SortedSet<EmptyEnum?>))]
        [InlineData(typeof(SortedSet<string[]>))]
        [InlineData(typeof(SortedSet<int[]>))]
        [InlineData(typeof(SortedSet<object[]>))]
        [InlineData(typeof(SortedSet<Version[]>))]
        [InlineData(typeof(SortedSet<int?[]>))]
        [InlineData(typeof(SortedSet<EmptyEnum?[]>))]
        public void IsSatisfiedBySortedSetRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new SortedSetSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}