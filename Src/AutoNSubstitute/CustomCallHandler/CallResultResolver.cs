using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <inheritdoc />
    public class CallResultResolver : ICallResultResolver
    {
        /// <summary>
        /// SpecimenContext used to resolve result and argument values.
        /// </summary>
        public ISpecimenContext SpecimenContext { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CallResultResolver"/>
        /// </summary>
        public CallResultResolver(ISpecimenContext specimenContext)
        {
            if (specimenContext == null) throw new ArgumentNullException(nameof(specimenContext));

            this.SpecimenContext = specimenContext;
        }

        /// <inheritdoc />
        public CallResultData ResolveResult(ICall callInfo)
        {
            var returnValue = this.ResolveReturnValue(callInfo);
            var returnMaybe = returnValue is OmitSpecimen ? Maybe.Nothing<object>() : Maybe.Just(returnValue);

            //Resolve ref/out parameter values.
            var argumentValues = new List<CallResultData.ArgumentValue>();
            var parameterInfos = callInfo.GetMethodInfo().GetParameters();
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
    }
}