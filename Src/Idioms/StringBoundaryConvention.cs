using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class StringBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            return new[] { new EmptyStringBehavior() };
        }

        #endregion
    }
}