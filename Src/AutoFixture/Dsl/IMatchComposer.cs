using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IMatchComposer<T> : ISpecimenBuilder
    {
        IMatchComposer<T> ByBaseType();

        IMatchComposer<T> ByExactType();

        IMatchComposer<T> ByParameterName(string name);

        IMatchComposer<T> ByPropertyName(string name);

        IMatchComposer<T> ByFieldName(string name);
    }
}
