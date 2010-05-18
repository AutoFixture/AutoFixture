using System;

namespace Ploeh.AutoFixture.Kernel
{
    public interface IFactoryComposer<T>
    {
        IPostprocessComposer<T> FromSeed(Func<T, T> factory);

        IPostprocessComposer<T> FromFactory(Func<T> factory);

        IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory);

        IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory);

        IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory);

        IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory);
    }
}
