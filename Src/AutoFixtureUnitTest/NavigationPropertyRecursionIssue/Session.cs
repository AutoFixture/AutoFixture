using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureUnitTest.NavigationPropertyRecursionIssue
{
    public class Session
    {
        public Language Language { get; set; }
    }
}
