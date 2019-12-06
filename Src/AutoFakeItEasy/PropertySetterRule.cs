using System;
using System.Linq;
using System.Reflection;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// A rule that intercepts property setter calls. Values will be saved into a result cache to be
    /// provided as the return value from the matching get methods when the latter are called.
    /// </summary>
    internal class PropertySetterRule : IFakeObjectCallRule
    {
        private readonly CallResultCache resultCache;

        public PropertySetterRule(CallResultCache resultCache)
        {
            this.resultCache = resultCache;
        }

        /// <summary>
        /// Gets the number of times this call rule is valid.
        /// </summary>
        /// <returns><c>null</c>, indicating that the rule has no expiration.</returns>
        public int? NumberOfTimesToCall => null;

        /// <summary>
        /// Gets whether this rule is applicable to the specified
        /// call. If <c>true</c> is returned then <see cref="Apply" /> will be called.
        /// </summary>
        /// <param name="fakeObjectCall">The call to check for applicability.</param>
        /// <returns><c>true</c> if the call is a property setter.</returns>
        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall)
        {
            return fakeObjectCall != null && IsSetter(fakeObjectCall.Method);
        }

        /// <summary>
        /// Stores the value provided in the property setter to be returned from later
        /// calls to the corresponding getter.
        /// </summary>
        /// <param name="interceptedFakeObjectCall">The call to apply the rule to.</param>
        public void Apply(IInterceptedFakeObjectCall interceptedFakeObjectCall)
        {
            if (interceptedFakeObjectCall == null) throw new ArgumentNullException(nameof(interceptedFakeObjectCall));

            var methodCall = CreateMethodCallForGetter(interceptedFakeObjectCall);
            this.resultCache.Put(methodCall, new MethodCallResult(interceptedFakeObjectCall.Arguments.Last()));
        }

        private static bool IsSetter(MethodInfo method) =>
            method.IsSpecialName &&
            method.Name.StartsWith("set_", StringComparison.Ordinal);

        private static MethodCall CreateMethodCallForGetter(IInterceptedFakeObjectCall fakeCall)
        {
            var methodName = "get_" + fakeCall.Method.Name.Substring(4);
            var numberOfArguments = fakeCall.Arguments.Count() - 1;

            var arguments = fakeCall.Arguments.Take(numberOfArguments);
            var parameters = fakeCall.Method.GetParameters().Take(numberOfArguments);
            return new MethodCall(fakeCall.Method.DeclaringType, methodName, parameters, arguments);
        }
    }
}