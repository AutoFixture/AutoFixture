using System.Collections.Generic;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public interface IMyInterface
    {
        IEnumerable<int> ThingNumbers { get; }

        void AddThing(Thing thing);
    }
}
