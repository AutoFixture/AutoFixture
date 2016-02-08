using System;

namespace Ploeh.AutoFixture.NUnit3
{
    public interface ITypedValueProvider
    {
        object CreateFrozenValue(Type type);
        object CreateValue(Type type);
    }
}