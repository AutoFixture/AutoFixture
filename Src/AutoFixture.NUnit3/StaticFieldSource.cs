using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace AutoFixture.NUnit3
{
    internal class StaticFieldSource : TestCaseSourceBase
    {
        public StaticFieldSource(FieldInfo field)
        {
            this.Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public FieldInfo Field { get; }

        public override IEnumerator GetEnumerator()
        {
            return GetFieldData(this.Field).GetEnumerator();
        }

        private static IEnumerable GetFieldData(FieldInfo field)
        {
            return field.GetValue(null) is IEnumerable enumerable
                ? enumerable
                : Enumerable.Empty<object>();
        }
    }
}