using System;
using AutoFixture.Kernel;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// A rule that intercepts method calls. Supplies the return and all out and ref values
    /// from the fixture. When a method is called repeatedly with the same arguments, the
    /// same return value and out and ref values will be provided.
    /// </summary>
    internal class MethodRule : IFakeObjectCallRule
    {
        private readonly ISpecimenContext context;
        private readonly CallResultCache resultSource;

        public MethodRule(ISpecimenContext context, CallResultCache resultSource)
        {
            this.context = context;
            this.resultSource = resultSource;
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
        /// <returns><c>true</c>. This rule applies to all methods.</returns>
        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall) => true;

        /// <summary>
        /// Applies an action to the call. If the method is not void, obtains a return value and sets it
        /// for the call. If there are any ref or out parameters, obtains values for them and sets them
        /// for the call.
        /// </summary>
        /// <param name="interceptedFakeObjectCall">The call to apply the rule to.</param>
        public void Apply(IInterceptedFakeObjectCall interceptedFakeObjectCall)
        {
            if (interceptedFakeObjectCall == null) throw new ArgumentNullException(nameof(interceptedFakeObjectCall));

            var callResult = this.resultSource.GetOrAdd(CreateMethodCall(interceptedFakeObjectCall), () => this.CreateMethodCallResult(interceptedFakeObjectCall));
            callResult.ApplyToCall(interceptedFakeObjectCall);
        }

        private static MethodCall CreateMethodCall(IInterceptedFakeObjectCall fakeCall)
        {
            var parameters = fakeCall.Method.GetParameters();
            return new MethodCall(fakeCall.Method.DeclaringType, fakeCall.Method.Name, parameters, fakeCall.Arguments);
        }

        private MethodCallResult CreateMethodCallResult(IInterceptedFakeObjectCall fakeObjectCall)
        {
            var result = new MethodCallResult(this.ResolveReturnValue(fakeObjectCall));
            this.AddOutAndRefValues(result, fakeObjectCall);
            return result;
        }

        private object ResolveReturnValue(IInterceptedFakeObjectCall fakeObjectCall)
        {
            var methodReturnType = fakeObjectCall.Method.ReturnType;
            return methodReturnType == typeof(void) ? null : this.context.Resolve(methodReturnType);
        }

        private void AddOutAndRefValues(MethodCallResult result, IInterceptedFakeObjectCall fakeObjectCall)
        {
            var parameters = fakeObjectCall.Method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterParameterType = parameters[i].ParameterType;
                if (parameterParameterType.IsByRef)
                {
                    var value = this.context.Resolve(parameterParameterType.GetElementType());
                    result.AddOutOrRefValue(i, value);
                }
            }
        }
    }
}