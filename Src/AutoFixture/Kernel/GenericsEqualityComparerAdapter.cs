using System.Collections;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Converts an <see cref="IEqualityComparer"/> into an
    /// <see cref="IEqualityComparer{T}"/> interface.
    /// </summary>
    class GenericsEqualityComparerAdapter : IEqualityComparer<object>
    {
        private readonly IEqualityComparer _comparer;

        public GenericsEqualityComparerAdapter(IEqualityComparer comparer)
        {
            _comparer = comparer;
        }

        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            return this._comparer.Equals(x, y);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return this._comparer.GetHashCode(obj);
        }
    }
}