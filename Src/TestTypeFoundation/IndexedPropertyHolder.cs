using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class IndexedPropertyHolder<T>
    {
        private readonly List<T> items;

        public IndexedPropertyHolder()
        {
            this.items = new List<T>();
        }

        public T this[int index]
        {
            get { return this.items[index]; }
            set { this.items[index] = value; }
        }
    }
}
