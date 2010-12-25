using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type == typeof(Guid))
            {
                yield return new GuidBoundaryBehavior();
            }
        }

        #endregion
    }
}