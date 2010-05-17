using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class StringGuardConvention : IValueGuardConvention
    {
        #region Implementation of IValueGuardConvention

        public IEnumerable<IInvalidValue> CreateInvalids(Fixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            return new IInvalidValue[] {
                new EmptyStringInvalidValue()
            };
        }

        #endregion
    }
}