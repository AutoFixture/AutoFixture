using System.Collections.Generic;

namespace AutoFixtureUnitTest.NavigationPropertyRecursionIssue
{
    public class Language
    {
        public ICollection<Session> Sessions { get; set; }
    }
}
