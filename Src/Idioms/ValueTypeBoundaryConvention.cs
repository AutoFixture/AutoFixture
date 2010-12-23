using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueTypeBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors()
        {
            return Enumerable.Empty<IBoundaryBehavior>();
        }

        #endregion
    }
}