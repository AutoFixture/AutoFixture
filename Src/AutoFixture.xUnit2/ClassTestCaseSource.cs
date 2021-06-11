using System;
using System.Collections;

namespace AutoFixture.Xunit2
{
    internal class ClassTestCaseSource : TestCaseSourceBase
    {
        private readonly Lazy<IEnumerable> lazyEnumerable;

        public ClassTestCaseSource(Type type, object[] parameters)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.lazyEnumerable = new Lazy<IEnumerable>(() => CreateInstance(this.Type, this.Parameters));
        }

        public Type Type { get; private set; }

        public object[] Parameters { get; private set; }

        public override IEnumerator GetEnumerator()
        {
            return this.lazyEnumerable.Value.GetEnumerator();
        }

        private static IEnumerable CreateInstance(Type type, object[] parameters)
        {
            var instance = Activator.CreateInstance(type, parameters);

            if (instance is not IEnumerable enumerable)
            {
                throw new InvalidOperationException(
                    $"Data source type \"{type}\" should implement the \"{typeof(IEnumerable)}\" interface.");
            }

            return enumerable;
        }
    }
}