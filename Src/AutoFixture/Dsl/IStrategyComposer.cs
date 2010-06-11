using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Dsl
{
    /// <summary>
    /// Provides statements that can be used to control how specimens are created and
    /// post-processed.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    public interface IStrategyComposer<T> : IFactoryComposer<T>, IPostprocessComposer<T>
    {
    }
}
