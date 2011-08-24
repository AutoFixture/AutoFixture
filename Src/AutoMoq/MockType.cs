using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    internal static class MockType
    {
        internal static bool IsMock(this Type type)
        {
            return (type != null
                && type.IsGenericType
                && typeof(Mock<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                && !type.GetMockedType().IsGenericParameter);
        }

        internal static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static ConstructorInfo GetParamsConstructor(this Type type)
        {
            return type.GetConstructor(new[] { typeof(object[]) });
        }

        internal static Type GetMockedType(this Type type)
        {
            return type.GetGenericArguments().Single();
        }
    }
}
