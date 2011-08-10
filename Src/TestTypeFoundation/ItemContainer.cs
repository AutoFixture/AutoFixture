using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class ItemContainer<T>
    {
        private readonly IEnumerable<T> items;

        public ItemContainer(params T[] items)
        {
            this.items = items;
        }

        public ItemContainer(IEnumerable<T> items)
            : this(items.ToArray())
        {
        }

        public ItemContainer(IList<T> items)
            : this(items.ToArray())
        {
        }

        public IEnumerable<T> Items
        {
            get { return this.items; }
        }
    }
}
