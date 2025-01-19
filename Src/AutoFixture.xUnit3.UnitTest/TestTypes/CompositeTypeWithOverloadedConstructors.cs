using System.Collections.Generic;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class CompositeTypeWithOverloadedConstructors<T>
    {
        public CompositeTypeWithOverloadedConstructors(IEnumerable<T> items)
        {
            this.Items = items;
        }

        public CompositeTypeWithOverloadedConstructors(params T[] items)
        {
            this.Items = items;
        }

        public CompositeTypeWithOverloadedConstructors(IList<T> items)
        {
            this.Items = items;
        }

        public IEnumerable<T> Items { get; }
    }
}
