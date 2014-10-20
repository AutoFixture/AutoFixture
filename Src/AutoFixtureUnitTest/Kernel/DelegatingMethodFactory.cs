using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DelegatingMethodFactory : IMethodFactory
    {
        public DelegatingMethodFactory()
        {
            this.OnCreate = m => null;
        }

        public IMethod Create(MethodInfo methodInfo)
        {
            return OnCreate(methodInfo);
        }

        internal Func<MethodInfo, IMethod> OnCreate { get; set; }
    }
}