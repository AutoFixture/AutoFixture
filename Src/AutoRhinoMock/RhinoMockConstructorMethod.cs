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
        private readonly Type mockTargetType;
        private readonly ParameterInfo[] paramInfos;

        public RhinoMockConstructorMethod(Type mockTargetType, ParameterInfo[] parameterInfos)
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
            this.paramInfos = parameterInfos;
        }

        #region IMethod Members

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            return MockRepository.GenerateMock(this.mockTargetType, new Type[0], parameters.ToArray());
        }

        #endregion
    }

}
