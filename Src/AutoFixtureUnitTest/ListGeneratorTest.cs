using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ListGeneratorTest
    {
        [Fact]
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
            Assert.True(expectedList.SequenceEqual(list));
            // Teardown
        }

        [Fact]
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
            Assert.True(expectedSequence.SequenceEqual(collection));
            // Teardown
        }
    }
}
