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
    }
}
