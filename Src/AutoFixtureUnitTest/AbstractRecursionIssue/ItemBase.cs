using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureUnitTest.AbstractRecursionIssue
{
    public abstract class ItemBase
    {
        public int ItemId { get; set; }

        public ICollection<ItemLocation> Locations { get; set; }
    }
}
