using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute.Core;
using NSubstitute.Routing;

// ReSharper disable InconsistentNaming - it improves readability of the code.
namespace AutoFixture.AutoNSubstitute
{
    /// <summary>
    ///     Helper to provide binary compatibility with different NSubstitute versions at run-time.
    /// </summary>
    internal static class CompatShim
    {
        private static readonly Func<ISubstituteState, ISubstitutionContext, ICallSpecificationFactory> CallSpecificationFactoryResolver = GetCallSpecificationFactoryResolver();

        private static readonly Func<ICallResults, ICall, bool> HasCallResultForResolver = GetHasCallResultForResolver();

        private static readonly Func<ICallHandler[], Route> RouteFactory = GetRouteFactory();

        public static ICallSpecificationFactory GetCallSpecificationFactory(ISubstituteState substituteState, ISubstitutionContext substitutionContext) =>
            CallSpecificationFactoryResolver.Invoke(substituteState, substitutionContext);

        public static bool CallResults_HasCallResultFor(ICallResults callResults, ICall call) =>
            HasCallResultForResolver.Invoke(callResults, call);

        public static Route Route_CreateNew(ICallHandler[] handlers) =>
            RouteFactory.Invoke(handlers);

        private static Func<ISubstituteState, ISubstitutionContext, ICallSpecificationFactory> GetCallSpecificationFactoryResolver()
        {
            /* Is optimized, as is invoked from the working code. */

            // Prior to v4
            var substituteState_CallSpecificationFactoryProp =
                typeof(ISubstituteState).GetTypeInfo().GetProperty("CallSpecificationFactory");
            if (substituteState_CallSpecificationFactoryProp != null)
            {
                var unboundDelegate =
                    MakeUnboundDelegate<ISubstituteState, ICallSpecificationFactory>(
                        substituteState_CallSpecificationFactoryProp.GetMethod);
                return (state, context) => unboundDelegate(state);
            }

            // Since v4
            var substitutionContext_CallSpecificationFactoryProp =
                typeof(ISubstitutionContext).GetTypeInfo().GetProperty("CallSpecificationFactory");
            if (substitutionContext_CallSpecificationFactoryProp != null)
            {
                var unboundDelegate =
                    MakeUnboundDelegate<ISubstitutionContext, ICallSpecificationFactory>(
                        substitutionContext_CallSpecificationFactoryProp.GetMethod);
                return (state, context) => unboundDelegate(context);
            }

            throw new InvalidOperationException("Cannot bind method.");
        }

        private static Func<ICallResults, ICall, bool> GetHasCallResultForResolver()
        {
            /* Do not optimize, as it's invoked from the obsolete API only. */

            // Prior to v4
            var hasResultForMethod = typeof(ICallResults).GetTypeInfo().GetMethod("HasResultFor");
            if (hasResultForMethod != null)
                return (callResults, call) => (bool)hasResultForMethod.Invoke(callResults, new object[] { call });

            // Since v4
            var tryGetResultMethod = typeof(ICallResults).GetTypeInfo().GetMethod("TryGetResult");
            if (tryGetResultMethod != null)
                return (callResults, call) => (bool)tryGetResultMethod.Invoke(callResults, new object[] { call, null });

            throw new InvalidOperationException("Cannot bind method.");
        }

        private static Func<ICallHandler[], Route> GetRouteFactory()
        {
            /* Do not optimize, as it's invoked from the obsolete API only. */

            var route = typeof(Route);

            // Since v4
            var constructor = route.GetTypeInfo().GetConstructor(new[] { typeof(ICallHandler[]) });
            if (constructor == null)
            {
                // Prior to v4
                constructor = route.GetTypeInfo().GetConstructor(new[] { typeof(IEnumerable<ICallHandler>) });
            }

            if (constructor == null)
                throw new InvalidOperationException("Unable to resolve Route constructor taking enumerable or array.");

            return callHandlers =>
                (Route)constructor.Invoke(new object[] { callHandlers });
        }

        private static Func<TThis, TResult> MakeUnboundDelegate<TThis, TResult>(MethodInfo mi) =>
            (Func<TThis, TResult>)mi.CreateDelegate(typeof(Func<TThis, TResult>));
    }
}