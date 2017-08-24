﻿using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class HashSetSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
#pragma warning disable 618
            var sut = new HashSetSpecification();
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
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        public void IsSatisfiedByNonHashSetRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
#pragma warning disable 618
            var sut = new HashSetSpecification();
#pragma warning restore 618
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(HashSet<object>))]
        [InlineData(typeof(HashSet<string>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(HashSet<Version>))]
        public void IsSatisfiedByCollectionRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
#pragma warning disable 618
            var sut = new HashSetSpecification();
#pragma warning restore 618
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}
