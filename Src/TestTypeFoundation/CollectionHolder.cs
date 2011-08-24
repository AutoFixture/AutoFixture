using System.Collections.Generic;

namespace Ploeh.TestTypeFoundation
{
    public class CollectionHolder<T>
    {
        public CollectionHolder()
        {
            this.Collection = new List<T>();
        }

        public ICollection<T> Collection { get; private set; }
    }
}
