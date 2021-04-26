using System.Collections.Generic;

namespace AutoFixture.Xunit2.UnitTest
{
    public class MyCollection<T>
    {
        private readonly List<T> items = new List<T>();

        public IEnumerable<T> Items => this.items;

        public void Add(T num) => this.items.Add(num);
    }
}
