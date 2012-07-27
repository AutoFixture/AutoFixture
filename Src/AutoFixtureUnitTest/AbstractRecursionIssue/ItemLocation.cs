using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureUnitTest.AbstractRecursionIssue
{
    public class ItemLocation
    {
        public int LocationId { get; set; }

        public ItemBase Item { get; set; }
    }
}
