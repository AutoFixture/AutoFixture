using System;
using System.Collections.ObjectModel;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="ObservableCollection{T}"/>.
    /// </summary>
    [Obsolete("This specification is obsolete. Use ExactTypeSpecification(typeof(ObservableCollection<>)) instead.")]
    public class ObservableCollectionSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="ObservableCollection{T}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type &&
                   type.TryGetSingleGenericTypeArgument(typeof(ObservableCollection<>), out var _);
        }
    }
}
