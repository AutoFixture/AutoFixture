using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidValueGuardConvention : IValueGuardConvention
    {
        #region Implementation of IValueGuardConvention

        public IEnumerable<IBoundaryBehavior> CreateInvalids(Fixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            return Enumerable.Repeat(new GuidBoundaryBehavior(), 1).Cast<IBoundaryBehavior>();
        }

        #endregion
    }
}