using System;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// A <see cref="ISpecification&lt;T&gt;"/> that is always <see langword="true"/>.
    /// </summary>
    public class TrueSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Determines whether the request is satisfied by the Specification.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><see langword="true"/></returns>
        public bool IsSatisfiedBy(T request)
        {
            return true;
        }
    }
}
