using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DelegatingConstructorQuery : IConstructorQuery
    {
        public DelegatingConstructorQuery()
        {
            this.OnSelectConstructors = t => Enumerable.Empty<ConstructorInfo>();
        }

        #region IConstructorPicker Members

        public IEnumerable<ConstructorInfo> SelectConstructors(Type type)
        {
            return this.OnSelectConstructors(type);
        }

        #endregion

        internal Func<Type, IEnumerable<ConstructorInfo>> OnSelectConstructors { get; set; }
    }
}
