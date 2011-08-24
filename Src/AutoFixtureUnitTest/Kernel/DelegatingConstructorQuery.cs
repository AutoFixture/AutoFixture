using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
#pragma warning disable 618
    public class DelegatingConstructorQuery : IConstructorQuery
#pragma warning restore 618
    {
        public DelegatingConstructorQuery()
        {
            this.OnSelectConstructors = t => Enumerable.Empty<IMethod>();
        }

        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return this.OnSelectConstructors(type);
        }

        internal Func<Type, IEnumerable<IMethod>> OnSelectConstructors { get; set; }
    }
}
