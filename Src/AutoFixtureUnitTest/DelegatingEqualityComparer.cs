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
            return obj.GetHashCode();
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
            return obj.GetHashCode();
        }

        internal Func<T, T, bool> OnEquals { get; set; }
    }
}