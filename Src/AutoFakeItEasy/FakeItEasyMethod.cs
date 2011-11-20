using System;
using System.Collections.Generic;
using System.Reflection;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    internal class FakeItEasyMethod : IMethod
    {
        private readonly ParameterInfo[] paramInfos;
        private readonly Type targetType;

        internal FakeItEasyMethod(Type targetType, ParameterInfo[] paramInfos)
        {
            if (paramInfos == null)
            {
                throw new ArgumentNullException("paramInfos");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.paramInfos = paramInfos;
            this.targetType = targetType;
        }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            return typeof(A)
                .GetMethod("Fake", new Type[] { })
                .MakeGenericMethod(new[] { this.targetType })
                .Invoke(null, null);
        }
    }
}
