using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    /// Encapsulates the access to a test data source type.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    public class ClassDataSource : DataSource
    {
        private readonly object[] parameters;

        /// <summary>
        /// Creates an instance of type <see cref="ClassDataSource" />.
        /// </summary>
        /// <param name="type">The test data source type.</param>
        /// <param name="parameters">Constructor arguments for the source type.</param>
        /// <exception cref="ArgumentNullException">Thrown when arguments are <see langword="null" />.</exception>
        public ClassDataSource(Type type, params object[] parameters)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// Gets the test data source type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the constructor parameters for test data source type.
        /// </summary>
        public IReadOnlyList<object> Parameters => Array.AsReadOnly(this.parameters);

         /// <inheritdoc />
        protected override IEnumerable<object[]> GetData()
        {
            var instance = Activator.CreateInstance(type: this.Type, args: this.parameters);
            if (instance is not IEnumerable<object[]> enumerable)
                throw new InvalidOperationException($"Data source type \"{this.Type}\" should implement the \"{typeof(IEnumerable<object>)}\" interface.");

            return enumerable;
        }
    }
}