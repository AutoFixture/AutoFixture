using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class ItemHolder<T1, T2>
    {
        private readonly IEnumerable<T1> t1s;
        private readonly IEnumerable<T2> t2s;

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
            this.t1s = t1s;
            this.t2s = t2s;
        }

        public IEnumerable<T1> Item1s
        {
            get { return this.t1s; }
        }

        public IEnumerable<T2> Item2s
        {
            get { return this.t2s; }
        }
    }

    /* Note that constructors must be unordered because this class is used to test that
     * constructors are correctly ordered by various implementations of IMethodQuery. For that
     * reason, please don't be a boy scout and order constructors 'nicely'. */
    public class ItemHolder<T>
    {
        private readonly IEnumerable<T> items;

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
            this.items = items;
        }

        public IEnumerable<T> Items
        {
            get { return this.items; }
        }
    }
}
