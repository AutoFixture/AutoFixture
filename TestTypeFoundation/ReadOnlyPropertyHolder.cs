using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class ReadOnlyPropertyHolder<T>
    {
        public T Property { get; private set; }
    }
}
