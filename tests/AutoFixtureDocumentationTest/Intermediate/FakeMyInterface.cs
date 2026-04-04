using System.Collections.Generic;
using System.Linq;

namespace AutoFixtureDocumentationTest.Intermediate;

public class FakeMyInterface : IMyInterface
{
    private readonly IList<Thing> _things;

    public FakeMyInterface()
    {
        _things = new List<Thing>();
    }

    public FakeMyInterface(int number, string text)
    {
        Number = number;
        Text = text;
    }

    public int Number { get; private set; }

    public string Text { get; private set; }

    public IEnumerable<int> ThingNumbers
    {
        get { return _things.Select(t => t.Number); }
    }

    public void AddThing(Thing thing)
    {
        _things.Add(thing);
    }
}