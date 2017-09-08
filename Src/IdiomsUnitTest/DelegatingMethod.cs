using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingMethod : IMethod
    {
        public DelegatingMethod()
        {
            this.Parameters = Enumerable.Empty<ParameterInfo>();
            this.OnInvoke = p => new object();
        }

        public Func<IEnumerable<object>, object> OnInvoke { get; set; }

        public IEnumerable<ParameterInfo> Parameters { get; set; }

        public object Invoke(IEnumerable<object> parameters)
        {
            return this.OnInvoke(parameters);
        }
    }
}
