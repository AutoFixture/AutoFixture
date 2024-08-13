using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    ///     Encapsulates access to a method that provides test cases.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    internal class MethodTestCaseSource : TestCaseSourceBase
    {
        private readonly object[] arguments;

        /// <summary>
        ///     Creates an instance of type <see cref="MethodTestCaseSource" />.
        /// </summary>
        /// <param name="methodInfo">The source method.</param>
        /// <param name="arguments">The source method arguments.</param>
        public MethodTestCaseSource(MethodInfo methodInfo, params object[] arguments)
        {
            this.MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        ///     Gets the source method info.
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        ///     Gets the source method arguments.
        /// </summary>
        public IReadOnlyList<object> Arguments => Array.AsReadOnly(this.arguments);

        /// <inheritdoc />
        public override IEnumerator GetEnumerator()
        {
            var value = this.MethodInfo.Invoke(null, this.arguments);

            if (value is not IEnumerable enumerable)
            {
                throw new InvalidCastException("Member does not return an enumerable value.");
            }

            return enumerable.GetEnumerator();
        }
    }
}