using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Contains extension methods to manipulate/setup instances of <see cref="Mock{T}"/>.
    /// </summary>
    public static class MockType
    {
        internal static bool IsMock(this Type type)
        {
            return type != null
                && type.GetTypeInfo().IsGenericType
                && typeof(Mock<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                && !type.GetMockedType().IsGenericParameter;
        }

        internal static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(ctor => !ctor.IsPrivate);
        }

        internal static ConstructorInfo GetParamsConstructor(this Type type)
        {
            return type.GetConstructor(new[] { typeof(object[]) });
        }

        internal static Type GetMockedType(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments().Single();
        }
    }
}
