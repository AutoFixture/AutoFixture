using System.Collections.Generic;

namespace TestTypeFoundation;

public class NonCompliantCollectionHolder<T>
{
    public NonCompliantCollectionHolder()
    {
        Collection = new List<T>();
    }

    public ICollection<T> Collection { get; set; }
}