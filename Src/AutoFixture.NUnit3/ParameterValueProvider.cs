using System;
using System.Linq;
using NUnit.Framework.Interfaces;

namespace Ploeh.AutoFixture.NUnit3
{
    public class ParameterValueProvider : IParameterValueProvider
    {
        private readonly ITypedValueProvider _typedValueProvider;

        public ParameterValueProvider(ITypedValueProvider typedValueProvider)
        {
            if (null == typedValueProvider)
            {
                throw new ArgumentNullException("typedValueProvider");
            }

            this._typedValueProvider = typedValueProvider;
        }

        public object CreateValueForParameter(IParameterInfo parameterInfo)
        {
            return HasFrozenAttribute(parameterInfo)
                ? this._typedValueProvider.CreateFrozenValue(parameterInfo.ParameterType)
                : this._typedValueProvider.CreateValue(parameterInfo.ParameterType);
        }

        private static bool HasFrozenAttribute(IReflectionInfo reflectionInfo)
        {
            return reflectionInfo.GetCustomAttributes<FrozenAttribute>(true).Any();
        }
    }
}