using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CollectionFillerCommandTest
    {
#pragma warning disable 618
        [Fact]
        public void AddManyToNullSpecimenThrows()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFillerCommand.AddMany(null, dummyContext));
            // Teardown
        }

        [Fact]
        public void AddManyWithNullContextThrows()
        {
            // Fixture setup
            var dummyCollection = new List<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFillerCommand.AddMany(dummyCollection, null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        public void AddManyToNonCollectionThrows(object specimen)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                CollectionFillerCommand.AddMany(specimen, dummyContext));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(LinkedList<int>))]
        [InlineData(typeof(DerivedCollection))]
        public void AddManyFillsCollection(Type collectionType)
        {
            // Fixture setup
            var collection = (ICollection<int>)Activator.CreateInstance(collectionType);

            var expectedRequest = new MultipleRequest(typeof(int));
            var expectedResult = Enumerable.Range(1, 3);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)expectedResult : new NoSpecimen(r) };
            // Exercise system
            CollectionFillerCommand.AddMany(collection, context);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(collection));
            // Teardown
        }
#pragma warning restore 618

        [Fact]
        public void SutIsSpecimenCommand()
        {
            var sut = new CollectionFillerCommand();
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        [Fact]
        public void ExecuteNullSpecimenThrows()
        {
            var sut = new CollectionFillerCommand();
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Execute(null, dummyContext));
        }

        [Fact]
        public void ExecuteNullContextThrows()
        {
            var sut = new CollectionFillerCommand();
            var dummyCollection = new List<object>();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Execute(dummyCollection, null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        public void ExecuteNonCollectionThrows(object specimen)
        {
            // Fixture setup
            var sut = new CollectionFillerCommand();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                sut.Execute(specimen, dummyContext));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(LinkedList<int>))]
        [InlineData(typeof(DerivedCollection))]
        public void ExecuteFillsCollection(Type collectionType)
        {
            // Fixture setup
            var collection = (ICollection<int>)Activator.CreateInstance(collectionType);

            var expectedRequest = new MultipleRequest(typeof(int));
            var expectedResult = Enumerable.Range(1, 3);
#pragma warning disable 618
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)expectedResult : new NoSpecimen(r) };
#pragma warning restore 618

            var sut = new CollectionFillerCommand();
            // Exercise system
            sut.Execute(collection, context);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(collection));
            // Teardown
        }

        [Fact]
        public void DoesNotThrowWithDuplicateEntries()
        {
            // Fixture setup
            var collection = new List<int>();

            var request = new MultipleRequest(typeof(int));
            var sequence = Enumerable.Repeat(0, 3);
#pragma warning disable 618
            var context = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? (object)sequence : new NoSpecimen(r) };
#pragma warning restore 618

            var sut = new CollectionFillerCommand();
            // Exercise system & Verify outcome
            Assert.DoesNotThrow(() => sut.Execute(collection, context));
            // Teardown
        }

        private class DerivedCollection : Collection<int>
        { }
    }
}
