using System;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public class Thing
    {
        public Thing()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public int Number { get; set; }

        public string Text { get; set; }
    }
}
