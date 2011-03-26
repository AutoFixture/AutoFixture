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

        public RhinoMockConstructorMethod(ConstructorInfo constructorInfo, ParameterInfo[] parameterInfos)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }
            if (parameterInfos == null)
            {
                throw new ArgumentNullException("parameterInfos");
            }

            this.ctor = constructorInfo;
            this.paramInfos = parameterInfos;
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
