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
        public ConstructingObjectBuilderTest()
        {
        }

        [Fact]
        public void CreateAnonymousWillReturnCreatedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            var sut = ConstructingObjectBuilderTest.CreateSut<object>(seed => expectedObject);
            // Exercise system
            object result = sut.CreateAnonymous();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void CreateManyWillCreateManyAnonymousItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedItemCount = fixture.RepeatCount;
            var sut = ConstructingObjectBuilderTest.CreateSut<PropertyHolder<int>>(fixture, seed => new PropertyHolder<int>());
            // Exercise system
            IEnumerable<PropertyHolder<int>> result = sut.CreateMany();
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
            var fixture = new Fixture();
            var sut = ConstructingObjectBuilderTest.CreateSut<PropertyHolder<int>>(fixture, seed => new PropertyHolder<int>());
            // Exercise system
            IEnumerable<PropertyHolder<int>> result = sut.CreateMany(expectedCount);
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
            var fixture = new Fixture();
            var expectedItemCount = fixture.RepeatCount;
            var sut = ConstructingObjectBuilderTest.CreateSut<string>(fixture, seed => seed + Guid.NewGuid());
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix);
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
            var fixture = new Fixture();
            fixture.RepeatCount = expectedItemCount;
            var sut = ConstructingObjectBuilderTest.CreateSut<string>(fixture, seed => seed + Guid.NewGuid());
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        private static ConstructingObjectBuilder<T> CreateSut<T>(Func<T, T> creator)
        {
            var f = new Fixture();
            return ConstructingObjectBuilderTest.CreateSut<T>(f, creator);
        }

        private static ConstructingObjectBuilder<T> CreateSut<T>(Fixture f, Func<T, T> creator)
        {
            return f.Build<T>().FromSeed(creator);
        }
    }
}
