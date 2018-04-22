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

        public int? NumberOfTimesToCall { get; } = null;

        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall) => true;

        public void Apply(IInterceptedFakeObjectCall fakeObjectCall)
        {
            if (fakeObjectCall == null) throw new ArgumentNullException(nameof(fakeObjectCall));

            SetReturnValue(fakeObjectCall);
            SetOutAndRefValues(fakeObjectCall);
        }

        private void SetReturnValue(IInterceptedFakeObjectCall fakeObjectCall)
        {
            var methodReturnType = fakeObjectCall.Method.ReturnType;
            if (methodReturnType != typeof(void))
            {
                var returnValue = this.context.Resolve(methodReturnType);
                fakeObjectCall.SetReturnValue(returnValue);
            }
        }

        private void SetOutAndRefValues(IInterceptedFakeObjectCall fakeObjectCall)
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