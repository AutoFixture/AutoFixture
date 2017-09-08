using System;

namespace Ploeh.AutoFixtureDocumentationTest.Greedy
{
    public class Bastard
    {
        public Bastard()
            : this(new DefaultFoo())
        {
        }

        public Bastard(IFoo foo)
        {
            if (foo == null)
            {
                throw new ArgumentNullException(nameof(foo));
            }

            this.Foo = foo;
        }

        public IFoo Foo { get; }
    }
}
