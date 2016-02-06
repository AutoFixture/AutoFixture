using System;

namespace Ploeh.AutoFixture.NUnit3
{
    public interface ITypedDataProvider
    {
        object CreateFrozenValue(Type type);
        object CreateValue(Type type);
    }
}