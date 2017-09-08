using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public class FakeMyInterface : IMyInterface
    {
        private readonly IList<Thing> things;

        public FakeMyInterface()
        {
            this.things = new List<Thing>();
        }

        public FakeMyInterface(int number, string text)
        {
            this.Number = number;
            this.Text = text;
        }

        public int Number { get; private set; }

        public string Text { get; private set; }

        public IEnumerable<int> ThingNumbers
        {
            get { return this.things.Select(t => t.Number); }
        }

        public void AddThing(Thing thing)
        {
            this.things.Add(thing);
        }
    }
}
