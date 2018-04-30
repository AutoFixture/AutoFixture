using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// A rule that intercepts property calls. Set values will be saved and
    /// provided as the return value from the matching get methods when the latter are called.
    /// If a get method is called before the corresponding set, the return value will
    /// be resolved from the fixture and similarly saved for later.
    /// </summary>
    internal class PropertyRule : IFakeObjectCallRule
    {
        private readonly ISpecimenContext context;

        private readonly ConcurrentDictionary<MethodCall, MethodCallResult> behaviors =
            new ConcurrentDictionary<MethodCall, MethodCallResult>();

        public PropertyRule(ISpecimenContext context)
        {
            this.context = context;
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
        /// <returns><c>true</c> if the call is a call to a property.</returns>
        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall)
        {
            return fakeObjectCall != null && IsProperty(fakeObjectCall.Method);
        }

        /// <summary>
        /// Applies an action to the call. If the call is a property setter, stores a value for later
        /// calls to the getter. If a getter, sets the return value to the stored value (if no stored value,
        /// obtains one and stores it first).
        /// </summary>
        /// <param name="interceptedFakeObjectCall">The call to apply the rule to.</param>
        public void Apply(IInterceptedFakeObjectCall interceptedFakeObjectCall)
        {
            if (interceptedFakeObjectCall == null) throw new ArgumentNullException(nameof(interceptedFakeObjectCall));

            var fakeObjectCall = new FakeObjectCall(interceptedFakeObjectCall);
            var methodCall = CreateMethodCall(fakeObjectCall);
            var methodReturnType = fakeObjectCall.Method.ReturnType;
            if (IsSetter(fakeObjectCall))
            {
                this.behaviors[methodCall] = new MethodCallResult(fakeObjectCall.Arguments.Last());
            }
            else
            {
                var returnValueAction = this.behaviors.GetOrAdd(
                    methodCall,
                    _ => new MethodCallResult(this.context.Resolve(methodReturnType)));
                returnValueAction.ApplyToCall(fakeObjectCall);
            }
        }

        private static bool IsProperty(MethodInfo method) =>
            method.IsSpecialName && (method.Name.StartsWith("get_", StringComparison.Ordinal) ||
                                     method.Name.StartsWith("set_", StringComparison.Ordinal));

        private static bool IsSetter(FakeObjectCall fakeObjectCall) =>
            fakeObjectCall.Method.IsSpecialName &&
            fakeObjectCall.Method.Name.StartsWith("set_", StringComparison.Ordinal);

        private static MethodCall CreateMethodCall(FakeObjectCall fakeCall)
        {
            var methodName = fakeCall.Method.Name.Substring(4);
            var numberOfArguments = fakeCall.Arguments.Count();
            if (IsSetter(fakeCall))
            {
                --numberOfArguments;
            }

            var arguments = fakeCall.Arguments.Take(numberOfArguments);
            var parameterTypes = fakeCall.Method.GetParameters()
                .Take(numberOfArguments)
                .Select(p => p.ParameterType);
            return new MethodCall(methodName, parameterTypes, arguments);
        }
    }
}