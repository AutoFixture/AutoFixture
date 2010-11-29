using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class NullValueGuardConvention : IValueGuardConvention
    {
        #region IValueGuardConvention Members

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Fixture fixture)
        {
            return Enumerable.Empty<IBoundaryBehavior>();
        }

        #endregion
    }
}
