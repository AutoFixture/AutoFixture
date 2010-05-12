using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class EqualityResponder
    {
        private readonly bool equals;

        public EqualityResponder(bool equals)
        {
            this.equals = equals;
        }

        public override bool Equals(object obj)
        {
            return this.equals;
        }

        public override int GetHashCode()
        {
            return this.equals.GetHashCode();
        }
    }
}
