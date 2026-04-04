using System.Collections.Generic;

namespace TestTypeFoundation;

public class IndexedPropertyHolder<T>
{
    private readonly List<T> _items;

    public IndexedPropertyHolder()
    {
        _items = new List<T>();
    }

    public T this[int index]
    {
        get { return _items[index]; }
        set { _items[index] = value; }
    }
}