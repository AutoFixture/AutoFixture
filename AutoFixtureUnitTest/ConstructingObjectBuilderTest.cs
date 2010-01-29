using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ConstructingObjectBuilderTest
    {
        public ConstructingObjectBuilderTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillReturnCreatedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            var sut = ConstructingObjectBuilderTest.CreateSut<object>(seed => expectedObject);
            // Exercise system
            object result = sut.CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<object>(expectedObject, result, "Created object");
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<int>(expectedItemCount, uniqueItemCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<int>(expectedCount, uniqueItemCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<int>(expectedItemCount, actualCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<int>(expectedItemCount, actualCount, "CreateMany");
            // Teardown
        }

        private static ConstructingObjectBuilder<T> CreateSut<T>(Func<T, T> creator)
        {
            var f = new Fixture();
            return ConstructingObjectBuilderTest.CreateSut<T>(f, creator);
        }

        private static ConstructingObjectBuilder<T> CreateSut<T>(Fixture f, Func<T, T> creator)
        {
#pragma warning disable 618
            return new ConstructingObjectBuilder<T>(f.TypeMappings, f.RepeatCount, null, creator);
#pragma warning restore 618
        }
    }
}
