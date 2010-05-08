namespace Ploeh.AutoFixture
{
    using System;
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
            this.RecursionRequestInterceptor = (obj) => { return null; };
        }
    }
}