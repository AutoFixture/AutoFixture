using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class NSubstituteConstructorMethod : IMethod
    {
        private readonly Type mockTargetType;
        private readonly ParameterInfo[] parameterInfos;

        public NSubstituteConstructorMethod(Type mockTargetType, ParameterInfo[] parameterInfos)
        {
            if (mockTargetType == null)
            {
                throw new ArgumentNullException("mockTargetType");
            }
            if (parameterInfos == null)
            {
                throw new ArgumentNullException("parameterInfos");
            }

            this.mockTargetType = mockTargetType;
            this.parameterInfos = parameterInfos;
        }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return parameterInfos; }
        }

        public Type MockTargetType
        {
            get { return mockTargetType; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            return Substitute.For(new[] {mockTargetType}, parameters.ToArray());
        }
    }
}