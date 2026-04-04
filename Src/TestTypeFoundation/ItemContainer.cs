using System.Collections.Generic;
using System.Linq;

namespace TestTypeFoundation;

public class ItemContainer<T>
{
    public ItemContainer(params T[] items)
    {
        Items = items;
    }

    public ItemContainer(IEnumerable<T> items)
        : this(items.ToArray())
    {
    }

    public ItemContainer(IList<T> items)
        : this(items.ToArray())
    {
    }

    public IEnumerable<T> Items { get; }
}