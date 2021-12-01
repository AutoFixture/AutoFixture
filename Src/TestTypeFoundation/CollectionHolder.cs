using System.Collections.Generic;

namespace TestTypeFoundation
{
    public class CollectionHolder<T>
    {
        public CollectionHolder()
        {
            this.Collection = new List<T>();
        }

        public CollectionHolder(ICollection<T> collection)
        {
            this.Collection = collection;
        }

        public ICollection<T> Collection { get; private set; }
    }
}
