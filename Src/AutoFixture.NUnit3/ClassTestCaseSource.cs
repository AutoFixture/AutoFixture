using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace AutoFixture.NUnit3
{
    internal class ClassTestCaseSource : TestCaseSourceBase
    {
        private readonly Lazy<IEnumerable> lazyEnumerable;

        public ClassTestCaseSource(Type type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.lazyEnumerable = new Lazy<IEnumerable>(() => GetInstance(type));
        }

        public Type Type { get; }

        private IEnumerable TestCases => this.lazyEnumerable.Value;

        public override IEnumerator GetEnumerator()
        {
            return this.TestCases.GetEnumerator();
        }

        private static IEnumerable GetInstance(Type type)
        {
            var constructor = type.GetTypeInfo().GetConstructor(EmptyArray<Type>.Value);
            var instance = constructor?.Invoke(new object[0]);
            return instance is IEnumerable enumerable
                ? enumerable
                : Enumerable.Empty<object>();
        }
    }
}