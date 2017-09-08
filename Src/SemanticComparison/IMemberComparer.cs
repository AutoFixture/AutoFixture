using System.Collections;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Evaluates requests for comparison of a property and field for equality.
    /// </summary>
    public interface IMemberComparer : IEqualityComparer
    {
        /// <summary>
        /// Evaluates a request for comparison of a property.
        /// </summary>
        /// <param name="request">The request for comparison of a property.</param>
        /// <returns><see langword="true"/> if <paramref name="request"/> is
        /// satisfied by the comparison; otherwise, <see langword="false"/>.
        /// </returns>
        bool IsSatisfiedBy(PropertyInfo request);

        /// <summary>
        /// Evaluates a request for comparison of a field.
        /// </summary>
        /// <param name="request">The request for comparison of a field.</param>
        /// <returns> <see langword="true"/> if <paramref name="request"/> is
        /// satisfied by the comparison; otherwise, <see langword="false"/>.
        /// </returns>
        bool IsSatisfiedBy(FieldInfo request);
    }
}