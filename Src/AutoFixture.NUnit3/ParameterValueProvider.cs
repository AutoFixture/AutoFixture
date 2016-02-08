using System.Linq;
using NUnit.Framework.Interfaces;

namespace Ploeh.AutoFixture.NUnit3
{
    public class ParameterValueProvider : IParameterValueProvider
    {
        private readonly ITypedValueProvider _typedValueProvider;

        public ParameterValueProvider(ITypedValueProvider typedValueProvider)
        {
            this._typedValueProvider = typedValueProvider;
        }

        public object Get(IParameterInfo parameterInfo)
        {
            return IsFrozen(parameterInfo)
                ? this._typedValueProvider.CreateFrozenValue(parameterInfo.ParameterType)
                : this._typedValueProvider.CreateValue(parameterInfo.ParameterType);
        }

        private static bool IsFrozen(IReflectionInfo reflectionInfo)
        {
            return reflectionInfo.GetCustomAttributes<FrozenAttribute>(true).Any();
        }
    }
}