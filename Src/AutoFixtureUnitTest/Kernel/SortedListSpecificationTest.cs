using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SortedListSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
#pragma warning disable 618
            var sut = new SortedListSpecification();
#pragma warning restore 618
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
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
            // Fixture setup
#pragma warning disable 618
            var sut = new SortedListSpecification();
#pragma warning restore 618
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
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
            // Fixture setup
#pragma warning disable 618
            var sut = new SortedListSpecification();
#pragma warning restore 618
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}
