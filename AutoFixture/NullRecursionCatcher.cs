namespace Ploeh.AutoFixture
{
    using System;
    using System.Collections;
    using Kernel;

    /// <summary>
    /// Recursion handler that returns null at recursion points.
    /// </summary>
    public class NullRecursionCatcher : RecursionCatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullRecursionCatcher"/> class.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        public NullRecursionCatcher(ISpecimenBuilder builder) : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullRecursionCatcher"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">An IEqualitycomparer implementation to use when comparing requests to determine recursion.</param>
        public NullRecursionCatcher(ISpecimenBuilder builder, IEqualityComparer comparer) : base(builder, comparer)
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
    }
}