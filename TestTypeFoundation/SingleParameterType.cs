using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class SingleParameterType<T>
    {
        public SingleParameterType(T parameter)
        {
            this.Parameter = parameter;
        }

        public T Parameter { get; private set; }
    }
}
