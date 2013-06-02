using System;
using System.Collections;

namespace Ploeh.SemanticComparison.UnitTest
{
    internal class DelegatingEqualityComparer : IEqualityComparer
    {
        internal DelegatingEqualityComparer()
        {
            this.OnEquals = (x, y) => false;
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return this.OnEquals(x, y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }

        internal Func<object, object, bool> OnEquals { get; set; }
    }
}
