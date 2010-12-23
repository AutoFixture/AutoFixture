using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReferenceTypeBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors()
        {
            return new[] { new NullReferenceBehavior() };
        }

        #endregion
    }
}