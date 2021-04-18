using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace AutoFixture.NUnit3
{
    internal class StaticPropertySource : TestCaseSourceBase
    {
        public StaticPropertySource(PropertyInfo property)
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public PropertyInfo Property { get; }

        public override IEnumerator GetEnumerator()
        {
            return GetPropertyData(this.Property).GetEnumerator();
        }

        private static IEnumerable GetPropertyData(PropertyInfo property)
        {
            return property.GetValue(null) is IEnumerable enumerable
                ? enumerable
                : Enumerable.Empty<object>();
        }
    }
}