using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    internal class MyClassFixture : Fixture
    {
        internal MyClassFixture()
        {
            this.Things = new List<Thing>();
            this.Register<IMyInterface>(() =>
                {
                    var fake = new FakeMyInterface();
                    this.Things.ToList().ForEach(t =>
                        fake.AddThing(t));
                    return fake;
                });
        }

        internal IList<Thing> Things { get; private set; }
    }
}
