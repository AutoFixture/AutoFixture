using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IMatchComposer<T> : ISpecimenBuilder
    {
        IMatchComposer<T> BaseType();

        IMatchComposer<T> ExactType();

        IMatchComposer<T> ParameterName(string name);

        IMatchComposer<T> PropertyName(string name);
    }
}
