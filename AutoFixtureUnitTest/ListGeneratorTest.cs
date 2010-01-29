using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ListGeneratorTest
    {
        public ListGeneratorTest()
        {
        }

        [TestMethod]
        public void RepeatWillCreateTheCorrectNumberOfObjects()
        {
            // Fixture setup
            int expectedCount = 7;
            Func<object> function = () => new object();
            // Exercise system
            IEnumerable<object> result = function.Repeat(expectedCount);
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, result.Count(), "Repeat");
            // Teardown
        }

        [TestMethod]
        public void RepeatWillInvokeFunctionTheCorrectNumberOfTimes()
        {
            // Fixture setup
            int expectedCount = 4;
            // Exercise system
            int result = 0;
            Func<int> function = () => result++;
            function.Repeat(expectedCount);
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, result, "Repeat");
            // Teardown
        }

        [TestMethod]
        public void AddManyWillAddItemsToList()
        {
            // Fixture setup
            int anonymousCount = 5;
            IEnumerable<int> expectedList = Enumerable.Range(1, anonymousCount);
            List<int> list = new List<int>();
            // Exercise system
            int i = 0;
            list.AddMany(() => ++i, anonymousCount);
            // Verify outcome
            CollectionAssert.AreEqual(expectedList.ToList(), list, "AddMany");
            // Teardown
        }

        [TestMethod]
        public void AddManyWillAddItemsToCollection()
        {
            // Fixture setup
            int anonymousCount = 8;
            IEnumerable<int> expectedSequence = Enumerable.Range(1, anonymousCount);
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            int i = 0;
            collection.AddMany(() => ++i, anonymousCount);
            // Verify outcome
            CollectionAssert.AreEqual(expectedSequence.ToList(), collection.ToList(), "AddMany");
            // Teardown
        }
    }
}
