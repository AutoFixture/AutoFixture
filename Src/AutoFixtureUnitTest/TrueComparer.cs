using System.Collections.Generic;

namespace AutoFixtureUnitTest
{
    public class TrueComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return true;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
