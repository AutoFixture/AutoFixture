using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureUnitTest.AbstractRecursionIssue
{
    public class FunkyItem : ItemBase
    {
        public int Funkiness { get; set; }
    }
}
