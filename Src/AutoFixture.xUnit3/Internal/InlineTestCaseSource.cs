using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit.Sdk;

namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    ///     Provides test cases from a predefined collection of values.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    internal sealed class InlineTestCaseSource : ITestCaseSource
    {
        private readonly object[] values;

        /// <summary>
        ///     Creates an instance of type <see cref="InlineTestCaseSource" />.
        /// </summary>
        /// <param name="values">The collection of inline values.</param>
        /// <exception cref="ArgumentNullException">Thrown when the values collection is <see langword="null" />.</exception>
        public InlineTestCaseSource(object[] values)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        /// <summary>
        ///     The collection of inline values.
        /// </summary>
        public IReadOnlyCollection<object> Values => Array.AsReadOnly(this.values);

        /// <inheritdoc />
        public IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method, DisposalTracker disposalTracker)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return GetTestCasesEnumerable(method, this.values);

            static IEnumerable<object[]> GetTestCasesEnumerable(MethodBase method, object[] values)
            {
                var parameters = method.GetParameters();
                var inlineArgumentCount = Math.Min(parameters.Length, values.Length);
                yield return values.AsSpan(0, inlineArgumentCount).ToArray();
            }
        }
    }
}