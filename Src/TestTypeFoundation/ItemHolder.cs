using System.Collections.Generic;

namespace Ploeh.TestTypeFoundation
{
    public class ItemHolder<T1, T2>
    {
        public ItemHolder()
        {
        }

        public ItemHolder(T1 item)
            : this(new[] { item }, new T2[0])
        {
        }

        public ItemHolder(T2 item)
            : this(new T1[0], new[] { item })
        {
        }

        private ItemHolder(T1[] t1s, T2[] t2s)
        {
            this.Item1s = t1s;
            this.Item2s = t2s;
        }

        public IEnumerable<T1> Item1s { get; }

        public IEnumerable<T2> Item2s { get; }
    }

    /* Note that constructors must be unordered because this class is used to test that
     * constructors are correctly ordered by various implementations of IMethodQuery. For that
     * reason, please don't be a boy scout and order constructors 'nicely'. */
    public class ItemHolder<T>
    {
        public ItemHolder(T x, T y, T z)
            : this(new[] { x, y, z })
        {
        }

        public ItemHolder(T item)
            : this(new[] { item })
        {
        }

        public ItemHolder()
        {
        }

        public ItemHolder(T x, T y)
            : this(new[] { x, y })
        {
        }

        private ItemHolder(T[] items)
        {
            this.Items = items;
        }

        public IEnumerable<T> Items { get; }
    }
}
