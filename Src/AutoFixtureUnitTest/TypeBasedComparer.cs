using System.Collections.Generic;

namespace AutoFixtureUnitTest
{
    public class TypeBasedComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return object.Equals(x.GetType(), y.GetType());
        }

        public int GetHashCode(T obj)
        {
            return obj.GetType().GetHashCode();
        }
    }
}
