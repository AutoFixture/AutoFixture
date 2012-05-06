using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Decorates an <see cref="ISpecimenBuilder" /> with a
    /// <see cref="OmitOnRecursionGuard" />.
    /// </summary>
    public class OmitOnRecursionBehavior : ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder" /> with an
        /// <see cref="OmitOnRecursionGuard"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder" /> decorated with an
        /// <see cref="OmitOnRecursionGuard" />.
        /// </returns>
        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            return new OmitOnRecursionGuard(builder);
        }
    }
}
