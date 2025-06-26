using System.Collections.Generic;

namespace TestTypeFoundation
{
    public class NullCollectionHolder<T>
    {
        public NullCollectionHolder()
        {
            this.Collection = null;
        }

        public ICollection<T> Collection { get; private set; }
    }
}
