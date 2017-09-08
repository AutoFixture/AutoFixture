using System;
using System.Collections.Generic;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
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
            // Fixture setup
            var dummyCollection = new List<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFiller.AddManyTo(null, dummyCollection));
            // Teardown
        }

        [Fact]
        public void AddManyToCollectionWithRepeatCountWithNullFixtureThrows()
        {
            // Fixture setup
            var dummyCollection = new List<object>();
            var dummyRepeatCount = 37;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFiller.AddManyTo(null, dummyCollection, dummyRepeatCount));
            // Teardown
        }

        [Fact]
        public void AddManyToCollectionWithCreatorWithNullFixtureThrows()
        {
            // Fixture setup
            var dummyCollection = new List<object>();
            Func<object> dummyCreator = () => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CollectionFiller.AddManyTo(null, dummyCollection, dummyCreator));
            // Teardown
        }
    }
}
