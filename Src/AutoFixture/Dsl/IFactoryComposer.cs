using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IFactoryComposer<T> : ISpecimenBuilderComposer
    {
        IPostprocessComposer<T> FromSeed(Func<T, T> factory);

        IPostprocessComposer<T> FromFactory(Func<T> factory);

        IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory);

        IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory);
    }
}
