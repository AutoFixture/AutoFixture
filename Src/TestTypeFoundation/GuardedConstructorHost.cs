using System;

namespace Ploeh.TestTypeFoundation
{
    public class GuardedConstructorHost<T> where T : class
    {
        public GuardedConstructorHost(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Item = item;
        }

        public T Item { get; }
    }
}
