﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DictionaryFillerTest
    {
        [Fact]
        public void AddManyToNullSpecimenThrows()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                DictionaryFiller.AddMany(null, dummyContext));
            // Teardown
        }

        [Fact]
        public void AddManyWithNullContextThrows()
        {
            // Fixture setup
            var dummyDictionary = new Dictionary<object, object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                DictionaryFiller.AddMany(dummyDictionary, null));
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
        public void AddManyToNonDictionaryThrows(object specimen)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                DictionaryFiller.AddMany(specimen, dummyContext));
            // Teardown
        }

        [Fact]
        public void AddManyFillsDictionary()
        {
            // Fixture setup
            var dictionary = new Dictionary<int, string>();

            var expectedRequest = new MultipleRequest(typeof(KeyValuePair<int, string>));
            var expectedResult = Enumerable.Range(1, 3).Select(i => new KeyValuePair<int, string>(i, i.ToString()));
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)expectedResult : new NoSpecimen(r) };
            // Exercise system
            DictionaryFiller.AddMany(dictionary, context);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(dictionary));
            // Teardown
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
            // Fixture setup
            var sut = new DictionaryFiller();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                sut.Execute(specimen, dummyContext));
            // Teardown
        }

        [Fact]
        public void ExecuteFillsDictionary()
        {
            // Fixture setup
            var dictionary = new Dictionary<int, string>();

            var expectedRequest = new MultipleRequest(typeof(KeyValuePair<int, string>));
            var expectedResult = Enumerable.Range(1, 3).Select(i => new KeyValuePair<int, string>(i, i.ToString()));
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)expectedResult : new NoSpecimen(r) };

            var sut = new DictionaryFiller();
            // Exercise system
            sut.Execute(dictionary, context);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(dictionary));
            // Teardown
        }
    }
}
