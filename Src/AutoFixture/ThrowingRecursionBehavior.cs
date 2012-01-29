using System;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Decorates a <see cref="ISpecimenBuilder"/> with a <see cref="ThrowingRecursionGuard"/>.
    /// </summary>
    public class ThrowingRecursionBehavior : ISpecimenBuilderPipe, ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder"/> with a
        /// <see cref="ThrowingRecursionGuard"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder"/> decorated with a <see cref="NullRecursionGuard"/>.
        /// </returns>
        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            return new ThrowingRecursionGuard(builder);
        }

        public IEnumerable<ISpecimenBuilder> Pipe(IEnumerable<ISpecimenBuilder> builders)
        {
            yield return new ThrowingRecursionGuard(
                new CompositeSpecimenBuilder(
                    builders));
        }
    }
}
