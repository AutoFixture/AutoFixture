using System;
using System.Collections.Generic;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <summary>
    /// CallHandler for NSubstitute to provide auto-values from AutoFixture.
    /// It resolves return values and sets ref/out parameter values obtained from <see cref="ICallResultResolver"/>.
    /// Uses cache to resolve value only once per call with specified args.
    /// </summary>
    public class AutoFixtureValuesHandler : ICallHandler
    {
        /// <summary>
        /// Factory for the <see cref="ICallSpecification"/> instances used as a cache key.
        /// </summary>
        public ICallSpecificationFactory CallSpecificationFactory { get; }

        /// <summary>
        /// Cache used to store the already resolved call results.
        /// </summary>
        public ICallResultCache ResultCache { get; }

        /// <summary>
        /// Resolver used to obtain call result if not present in cache.
        /// </summary>
        public ICallResultResolver ResultResolver { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AutoFixtureValuesHandler"/> with
        /// related specimen context, results cache and specification factory.
        /// </summary>
        public AutoFixtureValuesHandler(ICallResultResolver resultResolver, ICallResultCache resultCache,
            ICallSpecificationFactory callSpecificationFactory)
        {
            if (resultResolver == null) throw new ArgumentNullException(nameof(resultResolver));
            if (resultCache == null) throw new ArgumentNullException(nameof(resultCache));
            if (callSpecificationFactory == null) throw new ArgumentNullException(nameof(callSpecificationFactory));

            this.ResultResolver = resultResolver;
            this.ResultCache = resultCache;
            this.CallSpecificationFactory = callSpecificationFactory;
        }

        /// <summary>
        /// Try to handle the call - set ref/out params and return value.
        /// </summary>
        public RouteAction Handle(ICall call)
        {
            if (call == null) throw new ArgumentNullException(nameof(call));

            // Don't care about concurrency. If race condition happens - simply use the latest result.
            CallResultData result;
            if (!this.ResultCache.TryGetResult(call, out result))
            {
                result = this.ResultResolver.ResolveResult(call);
                var callSpec = this.CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall);

                this.ResultCache.AddResult(callSpec, result);
            }

            var callArguments = call.GetArguments();
            var originalArguments = call.GetOriginalArguments();
            foreach (var argumentValue in result.ArgumentValues)
            {
                var argIndex = argumentValue.Index;

                // If ref/out value has been already modified (e.g. by When..Do), don't override that value.
                if (!ArgValueWasModified(callArguments[argIndex], originalArguments[argIndex]))
                {
                    callArguments[argIndex] = argumentValue.Value;
                }
            }

            return result.ReturnValue.Fold(RouteAction.Continue, RouteAction.Return);
        }

        private static bool ArgValueWasModified(object current, object original)
        {
            return !EqualityComparer<object>.Default.Equals(current, original);
        }
    }
}