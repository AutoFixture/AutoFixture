namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// A Specification that evaluates requests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Determines whether the request is satisfied by the Specification.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if the request is satisfied by the Specification;
        ///   otherwise, <c>false</c>.
        /// </returns>
        bool IsSatisfiedBy(T request);
    }
}