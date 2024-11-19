using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    /// <summary>
    /// Encapsulates access to a method that provides test cases.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    public class MethodTestCaseSource : TestCaseSource
    {
        /// <summary>
        /// Creates an instance of type <see cref="MethodTestCaseSource" />.
        /// </summary>
        /// <param name="methodInfo">The source method.</param>
        /// <param name="arguments">The source method arguments.</param>
        public MethodTestCaseSource(MethodInfo methodInfo, params object[] arguments)
        {
            this.MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Gets the source method info.
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// Gets the source method arguments.
        /// </summary>
        public object[] Arguments { get; }

        /// <inheritdoc />
        protected override IEnumerable<object[]> GetTestData()
        {
            var value = this.MethodInfo.Invoke(null, this.Arguments);
            if (value is not IEnumerable<object[]> enumerable)
                throw new InvalidCastException("Member does not return an enumerable value.");

            return enumerable;
        }
    }
}