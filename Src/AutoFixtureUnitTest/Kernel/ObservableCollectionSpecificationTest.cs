﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ObservableCollectionSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new ObservableCollectionSpecification();
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
        public void IsSatisfiedByNonEnumerableRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new ObservableCollectionSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(Dictionary<string, string>))]
        [InlineData(typeof(Stack<int>))]
        [InlineData(typeof(Collection<Version>))]
        public void IsSatisfiedByEnumerableNonObservableRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
            var sut = new ObservableCollectionSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ObservableCollection<object>))]
        [InlineData(typeof(ObservableCollection<string>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(ObservableCollection<Version>))]
        public void IsSatisfiedByEnumerableRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
            var sut = new ObservableCollectionSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}
