using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class CollectionSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new CollectionSpecification();
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
        [InlineData(typeof(IEnumerable<Version>))]
        [InlineData(typeof(ICollection<Version>))]
        [InlineData(typeof(IList<Version>))]
        [InlineData(typeof(ReadOnlyCollection<object>))]
        [InlineData(typeof(PrivateConstructorCollection))]
        public void IsSatisfiedByNonCollectionRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new CollectionSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Collection<object>))]
        [InlineData(typeof(Collection<string>))]
        [InlineData(typeof(Collection<int>))]
        [InlineData(typeof(Collection<Version>))]
        [InlineData(typeof(HashSet<object>))]
        [InlineData(typeof(HashSet<string>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(HashSet<Version>))]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<Version>))]
        [InlineData(typeof(ObservableCollection<object>))]
        [InlineData(typeof(ObservableCollection<string>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(ObservableCollection<Version>))]
        [InlineData(typeof(SortedSet<object>))]
        [InlineData(typeof(SortedSet<string>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(SortedSet<Version>))]
        [InlineData(typeof(LinkedList<object>))]
        [InlineData(typeof(LinkedList<string>))]
        [InlineData(typeof(LinkedList<int>))]
        [InlineData(typeof(LinkedList<Version>))]
        [InlineData(typeof(DerivedCollection))]
        public void IsSatisfiedByCollectionRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
            var sut = new CollectionSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        private class DerivedCollection : Collection<object>
        { }

        private class PrivateConstructorCollection : Collection<object>
        {
            private PrivateConstructorCollection() { }
        }
    }
}
