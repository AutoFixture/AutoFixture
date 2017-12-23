using System;
using System.Collections.Generic;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    /// <summary>
    /// These tests mostly deal with boundary cases (like null
    /// guards) that are specific to the extension methods.
    /// Implementation are covered elsewhere (most notable in
    /// FixtureTest).
    /// </summary>
    public class CollectionFillerTest
    {
        [Fact]
        public void AddManyToCollectionWithNullFixtureThrows()
        {
            // Arrange
            var dummyCollection = new List<object>();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFiller.AddManyTo(null, dummyCollection));
        }

        [Fact]
        public void AddManyToCollectionWithRepeatCountWithNullFixtureThrows()
        {
            // Arrange
            var dummyCollection = new List<object>();
            var dummyRepeatCount = 37;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFiller.AddManyTo(null, dummyCollection, dummyRepeatCount));
        }

        [Fact]
        public void AddManyToCollectionWithCreatorWithNullFixtureThrows()
        {
            // Arrange
            var dummyCollection = new List<object>();
            Func<object> dummyCreator = () => new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFiller.AddManyTo(null, dummyCollection, dummyCreator));
        }
    }
}
