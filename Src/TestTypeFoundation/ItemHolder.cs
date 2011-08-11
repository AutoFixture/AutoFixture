using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
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
