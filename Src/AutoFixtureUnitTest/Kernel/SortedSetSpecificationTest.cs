using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SortedSetSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
#pragma warning disable 618
            var sut = new SortedSetSpecification();
#pragma warning restore 618
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
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
            // Fixture setup.
#pragma warning disable 618
            var sut = new SortedSetSpecification();
#pragma warning restore 618
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
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
            // Fixture setup
#pragma warning disable 618
            var sut = new SortedSetSpecification();
#pragma warning restore 618
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}