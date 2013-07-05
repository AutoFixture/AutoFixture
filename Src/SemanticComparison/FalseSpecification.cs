using System;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// A <see cref="ISpecification&lt;T&gt;"/> that is always <see langword="false"/>.
    /// </summary>
    public class FalseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Determines whether the request is satisfied by the Specification.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><see langword="false"/></returns>
        public bool IsSatisfiedBy(T request)
        {
            return false;
        }
    }
}
