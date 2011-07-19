using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class InstanceMethod : IMethod
    {
        private readonly MethodInfo method;
        private readonly ParameterInfo[] parameters;

        public InstanceMethod(MethodInfo instanceMethod)
        {
            if (instanceMethod == null)
            {
                throw new ArgumentNullException("instanceMethod");
            }

            this.method = instanceMethod;
            this.parameters = this.method.GetParameters();
        }

        public MethodInfo Method
        {
            get { return this.method; }
        }

        #region IMethod Members

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
