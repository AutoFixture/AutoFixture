using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

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

        #region IConstructorPicker Members

        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return this.OnSelectConstructors(type);
        }

        #endregion

        internal Func<Type, IEnumerable<IMethod>> OnSelectConstructors { get; set; }
    }
}
