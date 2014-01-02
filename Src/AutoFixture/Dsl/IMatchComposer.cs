using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IMatchComposer<T> : ISpecimenBuilder
    {
        IMatchComposer<T> BaseType();

        IMatchComposer<T> ExactType();
    }
}
