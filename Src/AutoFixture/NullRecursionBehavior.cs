using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Decorates a <see cref="ISpecimenBuilder"/> with a <see cref="RecursionGuard"/> with <see cref="NullRecursionHandler"/>.
    /// </summary>
    public class NullRecursionBehavior : ISpecimenBuilderTransformation
    {
        private const int DefaultRecursionDepth = 1;
        private readonly int recursionDepth;

        /// <summary>
        /// Initializes new instance of the <see cref="NullRecursionBehavior" /> class with default recursion depth.
        /// The default recursion depth will cause null assignment on first recursion.
        /// </summary>
        public NullRecursionBehavior()
            : this(DefaultRecursionDepth)
        {
        }

        /// <summary>
        /// Initializes new instance of the <see cref="NullRecursionBehavior" /> class with specific recursion depth.
        /// </summary>
        /// <param name="recursionDepth">The recursion depth at which the request will be assigned null.</param>
        public NullRecursionBehavior(int recursionDepth)
        {
            this.recursionDepth = recursionDepth;
        }

        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder"/> with a
        /// <see cref="RecursionGuard"/> with <see cref="NullRecursionHandler"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder"/> decorated with a <see cref="RecursionGuard"/>.
        /// </returns>
        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return new RecursionGuard(builder, new NullRecursionHandler(), this.recursionDepth);
        }
    }
}
