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
        private readonly object owner;

        public InstanceMethod(MethodInfo instanceMethod, object owner)
        {
            if (instanceMethod == null)
            {
                throw new ArgumentNullException("instanceMethod");
            }
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.method = instanceMethod;
            this.parameters = this.method.GetParameters();
            this.owner = owner;
        }

        public MethodInfo Method
        {
            get { return this.method; }
        }

        public object Owner
        {
            get { return this.owner; }
        }

        #region IMethod Members

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            return this.method.Invoke(this.owner, parameters.ToArray());
        }

        #endregion
    }
}
