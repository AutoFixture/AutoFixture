using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel
{
    public class DelegatingMethodQuery : IMethodQuery
    {
        public DelegatingMethodQuery()
        {
            this.OnSelectMethods = t => Enumerable.Empty<IMethod>();
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return this.OnSelectMethods(type);
        }

        internal Func<Type, IEnumerable<IMethod>> OnSelectMethods { get; set; }
    }
}
