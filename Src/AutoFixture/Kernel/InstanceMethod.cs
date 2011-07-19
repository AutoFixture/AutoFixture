using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class InstanceMethod : IMethod
    {
        public InstanceMethod(MethodInfo instanceMethod)
        {
            if (instanceMethod == null)
            {
                throw new ArgumentNullException("instanceMethod");
            }
        }

        #region IMethod Members

        public IEnumerable<ParameterInfo> Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
