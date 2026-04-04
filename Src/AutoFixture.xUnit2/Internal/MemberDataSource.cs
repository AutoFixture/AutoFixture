#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal;

/// <summary>
/// Encapsulates access to a member that provides test data.
/// </summary>
public class MemberDataSource : IDataSource
{
    private readonly object[] _arguments;

    /// <summary>
    /// Creates an instance of type <see cref="MemberDataSource" />.
    /// </summary>
    /// <param name="type">The containing type of the member.</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="arguments">The arguments provided to the member.</param>
    /// <exception cref="ArgumentNullException">Thrown when arguments are <see langref="null" />.</exception>
    public MemberDataSource(Type type, string name, params object[] arguments)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        Source = GetTestDataSource();
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
    public IReadOnlyList<object> Arguments => Array.AsReadOnly(_arguments);

    /// <summary>
    /// Gets the test data source.
    /// </summary>
    protected DataSource Source { get; }

    /// <inheritdoc />
    public IEnumerable<object[]> GetData(MethodInfo method)
    {
        return Source.GetData(method);
    }

    private DataSource GetTestDataSource()
    {
        var sourceMember = Type.GetMember(Name,
                MemberTypes.Method | MemberTypes.Field | MemberTypes.Property,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
            .FirstOrDefault();

        if (sourceMember is null)
        {
            var message = string.Format(
                CultureInfo.CurrentCulture,
                "Could not find public static member (property, field, or method) named '{0}' on {1}",
                Name, Type.FullName);
            throw new ArgumentException(message);
        }

        var returnType = sourceMember.GetReturnType();
        if (!typeof(IEnumerable<object[]>).IsAssignableFrom(returnType))
        {
            var message = string.Format(
                CultureInfo.CurrentCulture,
                "Member {0} on {1} does not return IEnumerable<object[]>",
                Name, Type.FullName);
            throw new ArgumentException(message);
        }

        return sourceMember switch
        {
            FieldInfo fieldInfo => new FieldDataSource(fieldInfo),
            PropertyInfo propertyInfo => new PropertyDataSource(propertyInfo),
            MethodInfo methodInfo => new MethodDataSource(methodInfo, _arguments),
            _ => throw new InvalidOperationException("Unsupported member type.")
        };
    }
}