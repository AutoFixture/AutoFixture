#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    /// <summary>
    /// Encapsulates access to a member that provides test data.
    /// </summary>
    public class MemberDataSource : IDataSource
    {
        private readonly object[] arguments;

        /// <summary>
        /// Creates an instance of type <see cref="MemberDataSource" />.
        /// </summary>
        /// <param name="type">The containing type of the member.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="arguments">The arguments provided to the member.</param>
        /// <exception cref="ArgumentNullException">Thrown when arguments are <see langref="null" />.</exception>
        public MemberDataSource(Type type, string name, params object[] arguments)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            this.Source = this.GetTestDataSource();
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
        public IReadOnlyList<object> Arguments => Array.AsReadOnly(this.arguments);

        /// <summary>
        /// Gets the test data source.
        /// </summary>
        protected DataSource Source { get; }

        /// <inheritdoc />
        public IEnumerable<object[]> GetData(MethodInfo method)
        {
            return this.Source.GetData(method);
        }

        private DataSource GetTestDataSource()
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

            return sourceMember switch
            {
                FieldInfo fieldInfo => new FieldDataSource(fieldInfo),
                PropertyInfo propertyInfo => new PropertyDataSource(propertyInfo),
                MethodInfo methodInfo => new MethodDataSource(methodInfo, this.arguments),
                _ => throw new InvalidOperationException("Unsupported member type.")
            };
        }
    }
}