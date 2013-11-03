using System;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Decorates a <see cref="ISpecimenBuilder"/> with a <see cref="NullRecursionGuard"/>.
    /// </summary>
    public class NullRecursionBehavior : ISpecimenBuilderTransformation
    {
        private readonly int? recursionDepth;

        /// <summary>
        /// Initializes new instance of the <see cref="NullRecursionBehavior" /> class with default recursion depth.
        /// </summary>
        public NullRecursionBehavior()
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
        /// <see cref="NullRecursionGuard"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder"/> decorated with a <see cref="RecursionGuard"/>.
        /// </returns>
        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            var recursionGuard = new RecursionGuard(builder, new NullRecursionHandler());
            if (recursionDepth.HasValue)
                recursionGuard.RecursionDepth = recursionDepth.Value;
            return recursionGuard;
        }
    }
}
