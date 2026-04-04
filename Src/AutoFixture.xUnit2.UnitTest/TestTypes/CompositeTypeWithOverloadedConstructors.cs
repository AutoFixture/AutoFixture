using System.Collections.Generic;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

public class CompositeTypeWithOverloadedConstructors<T>
{
    public CompositeTypeWithOverloadedConstructors(IEnumerable<T> items)
    {
        Items = items;
    }

    public CompositeTypeWithOverloadedConstructors(params T[] items)
    {
        Items = items;
    }

    public CompositeTypeWithOverloadedConstructors(IList<T> items)
    {
        Items = items;
    }

    public IEnumerable<T> Items { get; }
}