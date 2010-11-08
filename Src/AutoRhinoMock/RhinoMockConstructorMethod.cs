using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    public class RhinoMockConstructorMethod: IMethod
    {
        private readonly ConstructorInfo ctor;
        private readonly ParameterInfo[] paramInfos;

        internal RhinoMockConstructorMethod(ConstructorInfo ctor, ParameterInfo[] paramInfos)
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
            return MockRepository.GenerateMock(this.ctor.DeclaringType, Enumerable.Empty<Type>().ToArray(), parameters.ToArray());
        }

        #endregion
    }

}
