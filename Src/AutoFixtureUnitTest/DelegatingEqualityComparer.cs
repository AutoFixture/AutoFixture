using System;
using System.Collections;
using System.Collections.Generic;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DelegatingEqualityComparer : IEqualityComparer
    {
        public DelegatingEqualityComparer()
        {
            this.OnEquals = (x, y) => false;
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return this.OnEquals(x, y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            // It's not safe to assume anything about how OnEquals is going to [effectively] 
            // 'bucket' results, so make no assumptions that could lead to false negatives. 
            // See http://stackoverflow.com/a/3719617
            return 0;
        }

        internal Func<object, object, bool> OnEquals { get; set; }
    }

    internal class DelegatingEqualityComparer<T> : IEqualityComparer<T>
    {
        public DelegatingEqualityComparer()
        {
            this.OnEquals = (x, y) => false;
        }

        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return this.OnEquals(x, y);
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            // It's not safe to assume anything about how OnEquals is going to [effectively] 
            // 'bucket' results, so make no assumptions that could lead to false negatives. 
            // See http://stackoverflow.com/a/3719617
            return 0;
        }

        internal Func<T, T, bool> OnEquals { get; set; }
    }
}