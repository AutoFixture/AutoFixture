using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IStrategyComposer<T> : IFactoryComposer<T>, IPostprocessComposer<T>
    {
    }
}
