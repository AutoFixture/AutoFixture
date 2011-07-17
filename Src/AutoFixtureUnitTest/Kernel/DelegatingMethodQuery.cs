using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DelegatingMethodQuery : IMethodQuery
    {
        public DelegatingMethodQuery()
        {
            this.OnSelectMethods = t => Enumerable.Empty<IMethod>();
        }

        #region IMethodQuery Members

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return this.OnSelectMethods(type);
        }

        #endregion

        internal Func<Type, IEnumerable<IMethod>> OnSelectMethods { get; set; }
    }
}
