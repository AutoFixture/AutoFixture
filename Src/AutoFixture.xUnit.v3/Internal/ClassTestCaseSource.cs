using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoFixture.Xunit.v3.Internal
{
    /// <summary>
    ///     Encapsulates the access to a test case source type.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    internal class ClassTestCaseSource : TestCaseSourceBase
    {
        private readonly Lazy<IEnumerable> lazyEnumerable;
        private readonly object[] parameters;

        /// <summary>
        ///     Creates an instance of type <see cref="ClassTestCaseSource" />.
        /// </summary>
        /// <param name="type">The test case source type.</param>
        /// <param name="parameters">Constructor arguments for the source type.</param>
        /// <exception cref="ArgumentNullException">Thrown when arguments are <see langword="null" />.</exception>
        public ClassTestCaseSource(Type type, object[] parameters)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            this.lazyEnumerable = new Lazy<IEnumerable>(() => CreateInstance(this.Type, this.parameters), true);
        }

        /// <summary>
        ///     Gets the test case source type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///     Gets the constructor parameters for test case source type.
        /// </summary>
        public IReadOnlyList<object> Parameters => this.parameters;

        /// <inheritdoc />
        public override IEnumerator GetEnumerator()
        {
            return this.lazyEnumerable.Value.GetEnumerator();
        }

        private static IEnumerable CreateInstance(Type type, object[] parameters)
        {
            var instance = Activator.CreateInstance(type: type, args: parameters);

            if (instance is not IEnumerable enumerable)
            {
                throw new InvalidOperationException($"Data source type \"{type}\" should implement the \"{typeof(IEnumerable)}\" interface.");
            }

            return enumerable;
        }
    }
}