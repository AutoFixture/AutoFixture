using System;
using System.Collections;
using System.Collections.Generic;

namespace Ploeh.SemanticComparison.UnitTest
{
    internal class DelegatingEqualityComparer : IEqualityComparer
    {
        internal DelegatingEqualityComparer()
        {
            this.OnEquals = (x, y) => false;
            this.OnGetHashCode = x => 0;
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return this.OnEquals(x, y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return this.OnGetHashCode(obj);
        }

        internal Func<object, object, bool> OnEquals { get; set; }
 
        internal Func<object, int> OnGetHashCode { get; set; }
    }

    internal class DelegatingEqualityComparer<T> : IEqualityComparer<T>
    {
        public DelegatingEqualityComparer()
        {
            this.OnEquals = (x, y) => false;
            this.OnGetHashCode = x => 0;
        }

        public bool Equals(T x, T y)
        {
            return this.OnEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return this.OnGetHashCode(obj);
        }

        internal Func<T, T, bool> OnEquals { get; set; }

        internal Func<T, int> OnGetHashCode { get; set; }
    }
}
