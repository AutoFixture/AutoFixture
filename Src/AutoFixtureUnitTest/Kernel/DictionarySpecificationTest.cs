using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DictionarySpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new DictionarySpecification();
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
        [InlineData(typeof(IDictionary<object, object>))]
        [InlineData(typeof(IEnumerable<KeyValuePair<object, object>>))]
        [InlineData(typeof(PrivateConstructorDictionary))]
        public void IsSatisfiedByNonDictionaryRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new DictionarySpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Dictionary<object, object>))]
        [InlineData(typeof(Dictionary<int, string>))]
        [InlineData(typeof(Dictionary<string, int>))]
        [InlineData(typeof(Dictionary<Version, OperatingSystem>))]
        [InlineData(typeof(SortedDictionary<object, object>))]
        [InlineData(typeof(SortedDictionary<int, string>))]
        [InlineData(typeof(SortedDictionary<string, int>))]
        [InlineData(typeof(SortedDictionary<Version, OperatingSystem>))]
        [InlineData(typeof(SortedList<object, object>))]
        [InlineData(typeof(SortedList<int, string>))]
        [InlineData(typeof(SortedList<string, int>))]
        [InlineData(typeof(SortedList<Version, OperatingSystem>))]
        [InlineData(typeof(ConcurrentDictionary<object, object>))]
        [InlineData(typeof(ConcurrentDictionary<int, string>))]
        [InlineData(typeof(ConcurrentDictionary<string, int>))]
        [InlineData(typeof(ConcurrentDictionary<Version, OperatingSystem>))]
        [InlineData(typeof(DerivedDictionary))]
        public void IsSatisfiedByDictionaryRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
            var sut = new DictionarySpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        private class DerivedDictionary : Dictionary<object, object>
        { }

        private class PrivateConstructorDictionary : Dictionary<object, object>
        {
            private PrivateConstructorDictionary()
            { }
        }
    }
}
