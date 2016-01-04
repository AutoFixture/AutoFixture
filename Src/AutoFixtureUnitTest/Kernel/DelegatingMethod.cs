using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DelegatingMethod : IMethod
    {
        public DelegatingMethod()
        {
            this.OnParameters = Enumerable.Empty<ParameterInfo>;
            this.OnInvoke = p => null;
        }

        public IEnumerable<ParameterInfo> Parameters => this.OnParameters();

        public object Invoke(IEnumerable<object> parameters)
        {
            return this.OnInvoke(parameters);
        }

        internal Func<IEnumerable<ParameterInfo>> OnParameters { get; set; }
        
        internal Func<IEnumerable<object>, object> OnInvoke { get; set; }
    }
}