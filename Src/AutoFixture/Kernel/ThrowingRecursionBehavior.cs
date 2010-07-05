using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class ThrowingRecursionBehavior : ISpecimenBuilderTransformation
    {
        #region ISpecimenBuilderTransformation Members

        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            return new ThrowingRecursionGuard(builder);
        }

        #endregion
    }
}
