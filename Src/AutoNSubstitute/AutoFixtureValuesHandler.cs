using System;
using System.Collections.Generic;
using NSubstitute.Core;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// CallHandler for NSubstitute to provide auto-values from AutoFixture.
    /// It resolves return values and sets ref/out parameter values.
    /// Void and non-void methods are handled.
    /// </summary>
    public class AutoFixtureValuesHandler : ICallHandler
    {
        private static readonly Tuple<int, object>[] EmptyValues = new Tuple<int, object>[0];

        private ICallSpecificationFactory CallSpecificationFactory { get; }
        private IResultsCache ResultsCache { get; }
        private ISpecimenContext SpecimenContext { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AutoFixtureValuesHandler"/> with
        /// related specimen context, results cache and specification factory.
        /// </summary>
        public AutoFixtureValuesHandler(ISpecimenContext specimenContext, IResultsCache resultsCache, ICallSpecificationFactory callSpecificationFactory)
        {
            SpecimenContext = specimenContext;
            ResultsCache = resultsCache;
            CallSpecificationFactory = callSpecificationFactory;
        }

        /// <summary>
        /// Tro to handle the call - set ref/out params and return value.
        /// </summary>
        public RouteAction Handle(ICall call)
        {
            //Don't care about concurrency. If race condition happened, use the latest result only.
            CachedCallResult cachedResult;
            if (!ResultsCache.TryGetResult(call, out cachedResult))
            {
                cachedResult = ResolveResultForCall(call);
                var callSpec  = CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall);

                ResultsCache.AddResult(callSpec, cachedResult);
            }

            var callArguments = call.GetArguments();
            var originalArguments = call.GetOriginalArguments();
            foreach (var argumentValue in cachedResult.ArgumentValues)
            {
                var argIndex = argumentValue.Item1;
                var resolvedValue = argumentValue.Item2;

                //If ref/out value has been already modified (e.g. by When..Do), don't override that value.
                if (ArgValueWasModified(callArguments[argIndex], originalArguments[argIndex])) continue;

                if (!(resolvedValue is OmitSpecimen))
                {
                    callArguments[argIndex] = resolvedValue;
                }
            }

            if (cachedResult.ReturnValue is OmitSpecimen)
            {
                return RouteAction.Continue();
            }

            return RouteAction.Return(cachedResult.ReturnValue);
        }

        private CachedCallResult ResolveResultForCall(ICall call)
        {
            var returnValue = ResolveReturnValue(call);

            //Resolve ref/out parameter values.
            List<Tuple<int, object>> argumentValues = null;

            var parameterInfos = call.GetParameterInfos();
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];

                if (!parameterInfo.ParameterType.IsByRef) continue;
                if (argumentValues == null) argumentValues = new List<Tuple<int, object>>();

                //Unwrap parameter type, becase it is Type&
                var value = SpecimenContext.Resolve(parameterInfo.ParameterType.GetElementType());
                argumentValues.Add(Tuple.Create(i, value));
            }

            return new CachedCallResult(returnValue, argumentValues?.ToArray() ?? EmptyValues);
        }

        private object ResolveReturnValue(ICall call)
        {
            if (call.GetReturnType() == typeof(void)) return null;

            //If this is a call to property getter, we resolve PropertyInfo rather than Type.
            var propertyInfo = call.GetMethodInfo().GetPropertyFromGetterCallOrNull();
            if (propertyInfo != null)
            {
                return SpecimenContext.Resolve(propertyInfo);
            }

            return SpecimenContext.Resolve(call.GetReturnType());
        }

        private static bool ArgValueWasModified(object current, object original)
        {
            return !EqualityComparer<object>.Default.Equals(current, original);
        }
    }
}