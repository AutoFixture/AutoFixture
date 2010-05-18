using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface IRuleComposer<T> : IFactoryComposer<T>, IPostprocessComposer<T>
    {
    }
}
