using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class OmitOnRecursionBehavior : ISpecimenBuilderTransformation
    {
        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            return new OmitOnRecursionGuard(builder);
        }
    }
}
