using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    public class RhinoMockConstructorQuery : IConstructorQuery
    {
        #region IConstructorQuery Members

        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return from ci in type.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new RhinoMockConstructorMethod(ci.DeclaringType, paramInfos) as IMethod;
        }

        #endregion
    }
}
