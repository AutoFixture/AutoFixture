using System;

namespace Ploeh.TestTypeFoundation
{
    public class GuardedConstructorHost<T> where T : class
    {
        private readonly T item;

        public GuardedConstructorHost(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.item = item;
        }

        public T Item
        {
            get { return this.item; }
        }
    }
}
