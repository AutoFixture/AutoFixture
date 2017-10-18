using System.Collections.Generic;

namespace AutoFixtureUnitTest.AbstractRecursionIssue
{
    public abstract class ItemBase
    {
        public int ItemId { get; set; }

        public ICollection<ItemLocation> Locations { get; set; }
    }
}
