using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class SortedListSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new SortedListSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Theory]
        [InlineData(typeof(SortedList<int, string>))]
        [InlineData(typeof(SortedList<string, int>))]
        [InlineData(typeof(SortedList<object, Version>))]
        [InlineData(typeof(SortedList<Version, object>))]
        [InlineData(typeof(SortedList<int?, EmptyEnum?>))]
        [InlineData(typeof(SortedList<EmptyEnum?, int?>))]
        [InlineData(typeof(SortedList<string[], int[]>))]
        [InlineData(typeof(SortedList<int[], string[]>))]
        [InlineData(typeof(SortedList<object[], Version[]>))]
        [InlineData(typeof(SortedList<Version[], object[]>))]
        [InlineData(typeof(SortedList<int?[], EmptyEnum?[]>))]
        [InlineData(typeof(SortedList<EmptyEnum?[], int?[]>))]
        public void IsSatisfiedBySortedListRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new SortedListSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]  // non-Type
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
        [InlineData(typeof(List<object>))]  // Generic, but not SortedList<,>
        [InlineData(typeof(Dictionary<object, Version>))]  // double parameter generic, that implements same interface
        public void IsSatisfiedByNonSortedListRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new SortedListSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }
    }
}
