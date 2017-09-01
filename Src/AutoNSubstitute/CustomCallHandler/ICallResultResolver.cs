using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <summary>
    /// Resolves result for the calls using AutoFixture context.
    /// </summary>
    public interface ICallResultResolver
    {
        /// <summary>
        /// Resolve result for the call.
        /// </summary>
        CallResultData ResolveResult(ICall callInfo);
    }
}