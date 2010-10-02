using System;
using System.Globalization;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.AutoMoq
{
    internal class MockConstructorMethod : IMethod
    {
        private readonly ConstructorInfo ctor;
        private readonly ParameterInfo[] paramInfos;

        internal MockConstructorMethod(ConstructorInfo ctor, ParameterInfo[] paramInfos)
        {
            if (ctor == null)
            {
                throw new ArgumentNullException("ctor");
            }
            if (paramInfos == null)
            {
                throw new ArgumentNullException("paramInfos");
            }

            this.ctor = ctor;
            this.paramInfos = paramInfos;
        }

        #region IMethod Members

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            var paramsArray = new object[] { parameters };
            return this.ctor.Invoke(paramsArray);
        }

        #endregion
    }
}
