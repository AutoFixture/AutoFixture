using System;

namespace Ploeh.AutoFixtureDocumentationTest.Greedy
{
    public class Bastard
    {
        private readonly IFoo foo;

        public Bastard()
            : this(new DefaultFoo())
        {
        }

        public Bastard(IFoo foo)
        {
            if (foo == null)
            {
                throw new ArgumentNullException("foo");
            }

            this.foo = foo;
        }

        public IFoo Foo
        {
            get { return this.foo; }
        }
    }
}
