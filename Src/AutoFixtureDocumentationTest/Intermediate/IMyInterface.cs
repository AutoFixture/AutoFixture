using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public interface IMyInterface
    {
        IEnumerable<int> ThingNumbers { get; }

        void AddThing(Thing thing);
    }
}
