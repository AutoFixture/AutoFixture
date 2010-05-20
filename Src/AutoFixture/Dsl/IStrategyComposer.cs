using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IStrategyComposer<T> : IFactoryComposer<T>, IPostprocessComposer<T>
    {
    }
}
