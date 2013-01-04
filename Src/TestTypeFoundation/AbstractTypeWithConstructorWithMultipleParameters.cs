using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractTypeWithConstructorWithMultipleParameters<T1, T2>
    {
        private readonly T1 property1;
        private readonly T2 property2;

        protected AbstractTypeWithConstructorWithMultipleParameters(
            T1 parameter1,
            T2 parameter2)
        {
            this.property1 = parameter1;
            this.property2 = parameter2;
        }

        public T1 Property1
        {
            get { return property1; }
        }

        public T2 Property2
        {
            get { return property2; }
        }
    }
}
