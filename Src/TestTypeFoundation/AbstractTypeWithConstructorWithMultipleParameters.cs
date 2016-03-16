using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractTypeWithConstructorWithMultipleParameters<T1, T2>
    {
        protected AbstractTypeWithConstructorWithMultipleParameters(
            T1 parameter1,
            T2 parameter2)
        {
            this.Property1 = parameter1;
            this.Property2 = parameter2;
        }

        public T1 Property1 { get; }

        public T2 Property2 { get; }
    }
}
