using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IMatchComposer : ISpecimenBuilder
    {
        IMatchComposer Or { get; }

        IMatchComposer ByBaseType(Type targetType);

        IMatchComposer ByInterfaces(Type targetType);

        IMatchComposer ByExactType(Type targetType);

        IMatchComposer ByParameterName(Type targetType, string name);

        IMatchComposer ByPropertyName(Type targetType, string name);

        IMatchComposer ByFieldName(Type targetType, string name);
    }

    public interface IMatchComposer<out T> : IMatchComposer
    {
        new IMatchComposer<T> Or { get; }

        IMatchComposer<T> ByBaseType();

        IMatchComposer<T> ByInterfaces();

        IMatchComposer<T> ByExactType();

        IMatchComposer<T> ByParameterName(string name);

        IMatchComposer<T> ByPropertyName(string name);

        IMatchComposer<T> ByFieldName(string name);
    }
}
