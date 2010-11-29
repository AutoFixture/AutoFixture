using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReferenceTypeGuardConvention : IValueGuardConvention
    {
        #region Implementation of IValueGuardConvention

        public IEnumerable<IBoundaryBehavior> CreateInvalids(Fixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            return new IBoundaryBehavior[] {
                new NullReferenceBehavior()
            };
        }

        #endregion
    }
}