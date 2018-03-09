using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;

namespace AutoFixture.AutoFakeItEasy
{
    internal static class FakeItEasyType
    {
        internal static bool IsFake(this Type type)
        {
            return type != null
                   && type.GetTypeInfo().IsGenericType
                   && typeof(Fake<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                   && !type.GetFakedType().IsGenericParameter;
        }

        internal static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static Type GetFakedType(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments().Single();
        }
    }
}