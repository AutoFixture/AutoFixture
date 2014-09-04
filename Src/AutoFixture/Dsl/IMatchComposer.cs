using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IMatchComposer<T> : ISpecimenBuilder
    {
        IMatchComposer<T> Or { get; }

        IMatchComposer<T> ByBaseType();

        IMatchComposer<T> ByInterfaces();

        IMatchComposer<T> ByExactType();

        IMatchComposer<T> ByParameterName(string name);

        IMatchComposer<T> ByPropertyName(string name);

        IMatchComposer<T> ByFieldName(string name);
    }
}
