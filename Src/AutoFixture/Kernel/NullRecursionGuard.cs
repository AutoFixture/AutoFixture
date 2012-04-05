using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Recursion handler that returns null at recursion points.
    /// </summary>
    public class NullRecursionGuard : RecursionGuard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullRecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        public NullRecursionGuard(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullRecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">An IEqualitycomparer implementation to use when comparing requests to determine recursion.</param>
        public NullRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, comparer)
        {
        }

        /// <summary>
        /// Handles a request that would cause recursion by returning null.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>Always null.</returns>
        public override object HandleRecursiveRequest(object request)
        {
            return null;
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var builder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NullRecursionGuard(builder, this.Comparer);
        }
    }
}