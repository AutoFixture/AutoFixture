using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    internal static class FakeItEasyType
    {
        internal static bool IsFake(this Type type)
        {
            return (type != null
                && type.IsGenericType
                && typeof(Fake<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                && !type.GetFakedType().IsGenericParameter);
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
            return type.GetGenericArguments().Single();
        }

        internal static MethodInfo GetFakedMethod(this Type type)
        {
            return typeof(FakeBuilder<>)
                .MakeGenericType(type.GetGenericArguments())
                .GetMethod("Build", BindingFlags.Static | BindingFlags.NonPublic);
        }

        private static class FakeBuilder<T> where T : class
        {
            internal static Fake<T> Build(IEnumerable<object> argumentsForConstructor)
            {
                return typeof(T).IsInterface
                    ? new Fake<T>()
                    : new Fake<T>(o => o.WithArgumentsForConstructor(argumentsForConstructor));
            }
        }
    }
}