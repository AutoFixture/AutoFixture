using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Decorates a <see cref="ISpecimenBuilder"/> with a <see cref="RecursionGuard"/> with <see cref="ThrowingRecursionHandler"/>.
    /// </summary>
    public class ThrowingRecursionBehavior : ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder"/> with a
        /// <see cref="RecursionGuard"/> with <see cref="ThrowingRecursionHandler"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder"/> decorated with a <see cref="RecursionGuard" />.
        /// </returns>
        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return new RecursionGuard(builder, new ThrowingRecursionHandler());
        }
    }
}
