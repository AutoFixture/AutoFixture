using NUnit.Framework.Interfaces;

namespace Ploeh.AutoFixture.NUnit3
{
    public interface IParameterValueProvider
    {
        object CreateValueForParameter(IParameterInfo parameterInfo);
    }
}