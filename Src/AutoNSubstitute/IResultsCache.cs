using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Storage for already resolved call results.
    /// </summary>
    public interface IResultsCache
    {
        /// <summary>
        /// Adds result for passed specification.
        /// </summary>
        void AddResult(ICallSpecification callSpecification, CachedCallResult result);
        /// <summary>
        /// Returns the latest registered result that matches the passed calls.
        /// </summary>
        bool TryGetResult(ICall call, out CachedCallResult callResult);
    }
}