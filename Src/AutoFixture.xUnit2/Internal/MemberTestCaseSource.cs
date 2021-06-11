#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    /// <summary>
    /// Encapsulates access to a member that provides test cases.
    /// </summary>
    internal class MemberTestCaseSource : ITestCaseSource
    {
        private readonly object[] arguments;

        /// <summary>
        /// Creates an instance of type <see cref="MemberTestCaseSource" />.
        /// </summary>
        /// <param name="type">The containing type of the member.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="arguments">The arguments provided to the member.</param>
        /// <exception cref="ArgumentNullException">Thrown when arguments are <see langref="null" />.</exception>
        public MemberTestCaseSource(Type type, string name, params object[] arguments)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Gets the containing type of the member.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the arguments provided to the member.
        /// </summary>
        public IReadOnlyList<object> Arguments => this.arguments;

        /// <inheritdoc />
        public IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method)
        {
            var sourceMember = this.Type
                .GetMember(this.Name, BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault();

            ITestCaseSource source = sourceMember switch
            {
                FieldInfo fieldInfo => new FieldTestCaseSource(fieldInfo),
                PropertyInfo propertyInfo => new PropertyTestCaseSource(propertyInfo),
                MethodInfo methodInfo => new MethodTestCaseSource(methodInfo, this.arguments),
                _ => throw new InvalidOperationException("Unsupported member type.")
            };

            return source.GetTestCases(method);
        }
    }
}