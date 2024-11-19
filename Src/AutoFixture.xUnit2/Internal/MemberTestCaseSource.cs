#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    /// <summary>
    /// Encapsulates access to a member that provides test cases.
    /// </summary>
    public class MemberTestCaseSource : ITestCaseSource
    {
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
            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
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
        public object[] Arguments { get; }

        /// <inheritdoc />
        public IEnumerable<object[]> GetTestCases(MethodInfo method)
        {
            var sourceMember = this.Type.GetMember(this.Name,
                    MemberTypes.Method | MemberTypes.Field | MemberTypes.Property,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .FirstOrDefault();

            if (sourceMember is null)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "Could not find public static member (property, field, or method) named '{0}' on {1}",
                    this.Name, this.Type.FullName);
                throw new ArgumentException(message);
            }

            var returnType = sourceMember.GetReturnType();
            if (!typeof(IEnumerable<object[]>).IsAssignableFrom(returnType))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "Member {0} on {1} does not return IEnumerable<object[]>",
                    this.Name, this.Type.FullName);
                throw new ArgumentException(message);
            }

            TestCaseSource source = sourceMember switch
            {
                FieldInfo fieldInfo => new FieldTestCaseSource(fieldInfo),
                PropertyInfo propertyInfo => new PropertyTestCaseSource(propertyInfo),
                MethodInfo methodInfo => new MethodTestCaseSource(methodInfo, this.Arguments),
                _ => throw new InvalidOperationException("Unsupported member type.")
            };

            return source.GetTestCases(method);
        }
    }
}