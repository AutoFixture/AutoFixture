using System.Collections.Generic;

namespace TestTypeFoundation
{
    public class NonCompliantCollectionHolder<T>
    {
        public NonCompliantCollectionHolder()
        {
            this.Collection = new List<T>();
        }

        public ICollection<T> Collection { get; set; }
    }
}