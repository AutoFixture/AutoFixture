using System.Collections.Generic;
using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// CallHandler for NSubstitute to provide auto-values from AutoFixture.
    /// It resolves return values and sets ref/out parameter values.
    /// Void and non-void methods are handled.
    /// </summary>
    public class AutoFixtureValuesHandler : ICallHandler
    {
        public ICallSpecificationFactory CallSpecificationFactory { get; }
        public IResultsCache ResultsCache { get; }
        public ICallResultResolver ResultResolver { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AutoFixtureValuesHandler"/> with
        /// related specimen context, results cache and specification factory.
        /// </summary>
        public AutoFixtureValuesHandler(ICallResultResolver resultResolver, IResultsCache resultsCache,
            ICallSpecificationFactory callSpecificationFactory)
        {
            ResultResolver = resultResolver;
            ResultsCache = resultsCache;
            CallSpecificationFactory = callSpecificationFactory;
        }

        /// <summary>
        /// Tro to handle the call - set ref/out params and return value.
        /// </summary>
        public RouteAction Handle(ICall call)
        {
            //Don't care about concurrency. If race condition happened, use the latest result only.
            CallResultData cachedResult;
            if (!ResultsCache.TryGetResult(call, out cachedResult))
            {
                cachedResult = ResultResolver.ResolveResult(call);
                var callSpec = CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall);

                ResultsCache.AddResult(callSpec, cachedResult);
            }

            var callArguments = call.GetArguments();
            var originalArguments = call.GetOriginalArguments();
            foreach (var argumentValue in cachedResult.ArgumentValues)
            {
                var argIndex = argumentValue.Item1;
                var resolvedValue = argumentValue.Item2;

                //If ref/out value has been already modified (e.g. by When..Do), don't override that value.
                if (!ArgValueWasModified(callArguments[argIndex], originalArguments[argIndex]))
                {
                    callArguments[argIndex] = resolvedValue;
                }
            }

            return cachedResult.ReturnValue.Fold(RouteAction.Continue, RouteAction.Return);
        }

        private static bool ArgValueWasModified(object current, object original)
        {
            return !EqualityComparer<object>.Default.Equals(current, original);
        }
    }
}