using System;
using System.Collections.Generic;
using NSubstitute.Core;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <inheritdoc />
    public class CallResultResolver : ICallResultResolver
    {
        private static readonly Tuple<int, object>[] EmptyValues = new Tuple<int, object>[0];

        public ISpecimenContext SpecimenContext { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CallResultResolver"/>
        /// </summary>
        public CallResultResolver(ISpecimenContext specimenContext)
        {
            SpecimenContext = specimenContext;
        }

        public CallResultData ResolveResult(ICall call)
        {
            var returnValue = ResolveReturnValue(call);
            var returnMaybe = returnValue is OmitSpecimen ? Maybe.Nothing<object>() : Maybe.Just(returnValue);

            //Resolve ref/out parameter values.
            List<Tuple<int, object>> argumentValues = null;

            var parameterInfos = call.GetMethodInfo().GetParameters();
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];

                if (!parameterInfo.ParameterType.IsByRef) continue;

                //Unwrap parameter type, because it is Type&
                var value = SpecimenContext.Resolve(parameterInfo.ParameterType.GetElementType());
                if (value is OmitSpecimen) continue;

                if (argumentValues == null) argumentValues = new List<Tuple<int, object>>();
                argumentValues.Add(Tuple.Create(i, value));
            }

            return new CallResultData(returnMaybe, argumentValues?.ToArray() ?? EmptyValues);
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
    }
}