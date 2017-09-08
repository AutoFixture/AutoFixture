namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// A Specification that evaluates candidates.
    /// </summary>
    /// <typeparam name="T">The type of candidate to evaluate.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Determines whether the candidate is satisfied by the Specification.
        /// </summary>
        /// <param name="candidate">The request.</param>
        /// <returns>
        /// <see langword="true" /> if the candidate is satisfied by the
        /// Specification; otherwise, <see langword="false" />.
        /// </returns>
        bool IsSatisfiedBy(T candidate);
    }
}