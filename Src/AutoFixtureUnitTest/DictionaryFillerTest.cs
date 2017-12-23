using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DictionaryFillerTest
    {
        [Obsolete]
        public class Obsoleted
        {
            [Fact]
            public void AddManyToNullSpecimenThrows()
            {
                // Arrange
                var dummyContext = new DelegatingSpecimenContext();
                // Act & assert
                Assert.Throws<ArgumentNullException>(() =>
                    DictionaryFiller.AddMany(null, dummyContext));
            }

            [Fact]
            public void AddManyWithNullContextThrows()
            {
                // Arrange
                var dummyDictionary = new Dictionary<object, object>();
                // Act & assert
                Assert.Throws<ArgumentNullException>(() =>
                    DictionaryFiller.AddMany(dummyDictionary, null));
            }

            [Theory]
            [InlineData("")]
            [InlineData(1)]
            [InlineData(true)]
            [InlineData(false)]
            [InlineData(typeof(object))]
            [InlineData(typeof(string))]
            [InlineData(typeof(int))]
            public void AddManyToNonDictionaryThrows(object specimen)
            {
                // Arrange
                var dummyContext = new DelegatingSpecimenContext();
                // Act & assert
                Assert.Throws<ArgumentException>(() =>
                    DictionaryFiller.AddMany(specimen, dummyContext));
            }

            [Fact]
            public void AddManyFillsDictionary()
            {
                // Arrange
                var dictionary = new Dictionary<int, string>();

                var expectedRequest = new MultipleRequest(typeof(KeyValuePair<int, string>));
                var expectedResult = Enumerable.Range(1, 3).Select(i => new KeyValuePair<int, string>(i, i.ToString()));
                var context = new DelegatingSpecimenContext
                {
                    OnResolve = r => expectedRequest.Equals(r) ? (object)expectedResult : new NoSpecimen()
                };
                // Act
                DictionaryFiller.AddMany(dictionary, context);
                // Assert
                Assert.True(expectedResult.SequenceEqual(dictionary));
            }
        }

        [Fact]
        public void SutIsSpecimenCommand()
        {
            var sut = new DictionaryFiller();
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        [Fact]
        public void ExecuteNullSpecimenThrows()
        {
            var sut = new DictionaryFiller();
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Execute(null, dummyContext));
        }

        [Fact]
        public void ExecuteNullContextThrows()
        {
            var sut = new DictionaryFiller();
            var dummyDictionary = new Dictionary<object, object>();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Execute(dummyDictionary, null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        public void ExecuteNonDictionaryThrows(object specimen)
        {
            // Arrange
            var sut = new DictionaryFiller();
            var dummyContext = new DelegatingSpecimenContext();
            // Act & assert
            Assert.Throws<ArgumentException>(() =>
                sut.Execute(specimen, dummyContext));
        }

        [Fact]
        public void ExecuteFillsDictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            var expectedRequest = new MultipleRequest(typeof(KeyValuePair<int, string>));
            var expectedResult = Enumerable.Range(1, 3).Select(i => new KeyValuePair<int, string>(i, i.ToString()));
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)expectedResult : new NoSpecimen() };

            var sut = new DictionaryFiller();
            // Act
            sut.Execute(dictionary, context);
            // Assert
            Assert.True(expectedResult.SequenceEqual(dictionary));
        }

        [Fact]
        public void DoesNotThrowWithDuplicateEntries()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            var request = new MultipleRequest(typeof(KeyValuePair<int, string>));
            var sequence = Enumerable.Repeat(0, 3).Select(i => new KeyValuePair<int, string>(i, i.ToString()));
            var context = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? (object)sequence : new NoSpecimen() };

            var sut = new DictionaryFiller();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(dictionary, context)));
        }
    }
}
