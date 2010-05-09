using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class DoublePropertyHolder<T1, T2>
    {
        public T1 Property1 { get; set; }

        public T2 Property2 { get; set; }
    }
}
