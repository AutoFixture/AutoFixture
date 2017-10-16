namespace AutoFixture.Kernel
{
    /// <summary>
    /// A Specification that evaluates requests.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a codification of the Specification patter for requests. This interface can (and
    /// should) be used in any place where you need to filter requests for specimens.
    /// </para>
    /// </remarks>
    public interface IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is satisfied by the Specification;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        bool IsSatisfiedBy(object request);
    }
}
