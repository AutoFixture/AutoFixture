using System.Collections.Generic;
using System.Linq;
using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueTypeBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            return Enumerable.Empty<IBoundaryBehavior>();
        }

        #endregion
    }
}