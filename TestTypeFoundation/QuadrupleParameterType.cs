using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class QuadrupleParameterType<T1, T2, T3, T4>
    {
        public QuadrupleParameterType(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4)
        {
            this.Parameter1 = parameter1;
            this.Parameter2 = parameter2;
            this.Parameter3 = parameter3;
            this.Parameter4 = parameter4;
        }

        public T1 Parameter1 { get; private set; }

        public T2 Parameter2 { get; private set; }

        public T3 Parameter3 { get; private set; }

        public T4 Parameter4 { get; private set; }
    }
}
