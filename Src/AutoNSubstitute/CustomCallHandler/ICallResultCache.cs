using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <summary>
    /// Cache for already resolved call results.
    /// </summary>
    public interface ICallResultCache
    {
        /// <summary>
        /// Adds result for the passed <paramref name="callSpecification"/>.
        /// </summary>
        void AddResult(ICallSpecification callSpecification, CallResultData result);

        /// <summary>
        /// Returns the latest registered result that matches the passed call.
        /// </summary>
        bool TryGetResult(ICall callInfo, out CallResultData callResult);
    }
}