using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Recursion handler that returns null at recursion points.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    [Obsolete("This class will be removed in a future version of AutoFixture. Instead, use an instance of RecursionGuard, composed with an instance of NullRecursionHandler.", true)]
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
        [Obsolete("This class will be removed in a future version of AutoFixture. Instead, use an instance of RecursionGuard, composed with an instance of NullRecursionHandler.", true)]
        public override object HandleRecursiveRequest(object request)
        {
            return null;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var builder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NullRecursionGuard(builder, this.Comparer);
        }
    }
}