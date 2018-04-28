using System;
using AutoFixture.Kernel;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// A rule that intercepts method calls. Supplies the return and all out and ref values
    /// from the fixture. New values will be fetched from the fixture on each call.
    /// </summary>
    internal class MethodRule : IFakeObjectCallRule
    {
        private readonly ISpecimenContext context;

        public MethodRule(ISpecimenContext context)
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

            var fakeObjectCall = new FakeObjectCall(interceptedFakeObjectCall);
            SetReturnValue(fakeObjectCall);
            SetOutAndRefValues(fakeObjectCall);
        }

        private void SetReturnValue(FakeObjectCall fakeObjectCall)
        {
            var methodReturnType = fakeObjectCall.Method.ReturnType;
            if (methodReturnType != typeof(void))
            {
                var returnValue = this.context.Resolve(methodReturnType);
                fakeObjectCall.SetReturnValue(returnValue);
            }
        }

        private void SetOutAndRefValues(FakeObjectCall fakeObjectCall)
        {
            var parameters = fakeObjectCall.Method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterParameterType = parameters[i].ParameterType;
                if (parameterParameterType.IsByRef)
                {
                    var value = this.context.Resolve(parameterParameterType.GetElementType());
                    fakeObjectCall.SetArgumentValue(i, value);
                }
            }
        }
    }
}