using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IRuleComposer<T> : IFactoryComposer<T>, IPostprocessComposer<T>
    {
    }
}
