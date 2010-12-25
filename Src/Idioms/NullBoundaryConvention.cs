using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class NullBoundaryConvention : IBoundaryConvention
    {
        #region IBoundaryConvention Members

        public IEnumerable<ExceptionBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            return Enumerable.Empty<ExceptionBoundaryBehavior>();
        }

        #endregion
    }
}
