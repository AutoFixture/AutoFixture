using System.Collections.Generic;

namespace TestTypeFoundation
{
    public class ExtendedList<T> : List<T>
    {
        public void Add(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }
    }
}
