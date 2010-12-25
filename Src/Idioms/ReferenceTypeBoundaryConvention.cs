using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReferenceTypeBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        public IEnumerable<ExceptionBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsClass)
            {
                yield return new NullReferenceBehavior();
            }
        }

        #endregion
    }
}