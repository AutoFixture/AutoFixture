using System;

namespace TestTypeFoundation
{
    public class TypeWithRefMethod<T>
    {
        public void InvokeIt(T x, ref T y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
        }
    }
}
