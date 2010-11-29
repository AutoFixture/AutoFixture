using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueTypeGuardConvention : IValueGuardConvention
    {
        #region Implementation of IValueGuardConvention

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Fixture fixture)
        {
            return Enumerable.Empty<IBoundaryBehavior>();
        }

        #endregion
    }
}