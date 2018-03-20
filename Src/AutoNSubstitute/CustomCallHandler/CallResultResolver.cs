using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture.Kernel;
using NSubstitute;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <inheritdoc />
    public class CallResultResolver : ICallResultResolver
    {
        private static readonly Type DelegateCallType;
        private static readonly FieldInfo DelegateTypeFieldInfo;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "One field initialization depends on other one - order should be guaranteed.")]
        static CallResultResolver()
        {
            const string delegateCallTypeName = "NSubstitute.Proxies.DelegateProxy.DelegateCall";
            DelegateCallType = typeof(Substitute).GetTypeInfo().Assembly.GetType(delegateCallTypeName, throwOnError: false);
            DelegateTypeFieldInfo =
                DelegateCallType?.GetField("_delegateType", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// SpecimenContext used to resolve result and argument values.
        /// </summary>
        public ISpecimenContext SpecimenContext { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CallResultResolver"/>
        /// </summary>
        public CallResultResolver(ISpecimenContext specimenContext)
        {
            this.SpecimenContext = specimenContext ?? throw new ArgumentNullException(nameof(specimenContext));
        }

        /// <inheritdoc />
        public CallResultData ResolveResult(ICall callInfo)
        {
            var returnValue = this.ResolveReturnValue(callInfo);
            var returnMaybe = returnValue is OmitSpecimen ? Maybe.Nothing<object>() : Maybe.Just(returnValue);

            //Resolve ref/out parameter values.
            var argumentValues = new List<CallResultData.ArgumentValue>();
            var parameterInfos = GetMethodParameters(callInfo);
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];

                if (!parameterInfo.ParameterType.IsByRef) continue;

                // Unwrap parameter type, because it is Type& for ref/out methods.
                var value = this.SpecimenContext.Resolve(parameterInfo.ParameterType.GetElementType());
                if (value is OmitSpecimen) continue;

                argumentValues.Add(new CallResultData.ArgumentValue(i, value));
            }

            return new CallResultData(returnMaybe, argumentValues);
        }

        private object ResolveReturnValue(ICall call)
        {
            if (call.GetReturnType() == typeof(void)) return null;

            // If this is a call to a property getter, we resolve value via the 'PropertyInfo' request.
            var propertyInfo = call.GetMethodInfo().GetPropertyFromGetterCallOrNull();
            if (propertyInfo != null)
            {
                return this.SpecimenContext.Resolve(propertyInfo);
            }

            return this.SpecimenContext.Resolve(call.GetReturnType());
        }

        private static ParameterInfo[] GetMethodParameters(ICall call)
        {
            // A workaround for the older versions of NSubstitute to retrieve the original delegate signature.
            // The related issue has been fixed in v4, so no tweaks are required there.
            // The workaround will be self disabled in v4+ due to the internal NSubstitute refactoring.
            if (call.Target().GetType() == DelegateCallType && DelegateTypeFieldInfo != null)
            {
                var delegateType = (Type)DelegateTypeFieldInfo.GetValue(call.Target());
                return delegateType.GetMethod("Invoke").GetParameters();
            }

            return call.GetMethodInfo().GetParameters();
        }
    }
}