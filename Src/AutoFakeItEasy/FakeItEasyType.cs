using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;

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

        internal static IMethod GetFakedMethod(this Type type, ParameterInfo[] paramInfos)
        {
            Type builder = typeof(FakeBuilder<>).MakeGenericType(type.GetGenericArguments());
            var bindings = BindingFlags.Static | BindingFlags.NonPublic;

            if (paramInfos.Any())
            {
                return new UnknownParametersCountMethod(new StaticMethod(builder.GetMethod("BuildWithArgumentsForConstructor", bindings)), paramInfos);
            }

            return new StaticMethod(builder.GetMethod("Build", bindings));
        }

        private static class FakeBuilder<T> where T : class
        {
            internal static Fake<T> BuildWithArgumentsForConstructor(object[] argumentsForConstructor)
            {
                return new Fake<T>(o => o.WithArgumentsForConstructor(argumentsForConstructor));
            }

            internal static Fake<T> Build()
            {
                return new Fake<T>();
            }
        }
    }
}