using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq
{
    internal class MockConstructorMethod : IMethod
    {
        private readonly ConstructorInfo ctor;

        internal MockConstructorMethod(ConstructorInfo ctor, ParameterInfo[] paramInfos)
        {
            this.ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            this.Parameters = paramInfos ?? throw new ArgumentNullException(nameof(paramInfos));
        }

        public IEnumerable<ParameterInfo> Parameters { get; }

        public object Invoke(IEnumerable<object> parameters)
        {
            var paramsArray = new object[] { parameters };
            return this.ctor.Invoke(paramsArray);
        }
    }
}