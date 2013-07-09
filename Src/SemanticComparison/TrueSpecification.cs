using System;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// A <see cref="ISpecification&lt;T&gt;"/> that is always <see langword="true"/>.
    /// </summary>
    public class TrueSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Determines whether a candidate is satisfied by the Specification.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns><see langword="true" /></returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return true;
        }
    }
}
