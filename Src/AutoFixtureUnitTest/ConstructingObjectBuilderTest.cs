using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ConstructingObjectBuilderTest
    {
        [Fact]
        public void CreateAnonymousWillReturnCreatedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            var sut = new Fixture();
            // Exercise system
            object result = sut.Build<object>().FromSeed(seed => expectedObject).CreateAnonymous();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void CreateManyWillCreateManyAnonymousItems()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany();
            // Verify outcome
            var uniqueItemCount = (from ph in result
                                   select ph.Property).Distinct().Count();
            Assert.Equal<int>(expectedItemCount, uniqueItemCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            int expectedCount = 401;
            var sut = new Fixture();
            // Exercise system
            IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany(expectedCount);
            // Verify outcome
            var uniqueItemCount = (from ph in result
                                   select ph.Property).Distinct().Count();
            Assert.Equal<int>(expectedCount, uniqueItemCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Fixture setup
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 29;
            var sut = new Fixture();
            sut.RepeatCount = expectedItemCount;
            // Exercise system
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }
    }
}
