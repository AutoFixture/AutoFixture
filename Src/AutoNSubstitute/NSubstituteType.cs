using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal static class NSubstituteType
    {
        internal static IMethod GetSubstituteFactoryMethod(this Type type, params ParameterInfo[] paramInfos)
        {
            var builder = typeof(SubstituteBuilder<>).MakeGenericType(type);
            var bindings = BindingFlags.Static | BindingFlags.NonPublic;

            if (paramInfos.Any())
                return new UnknownParametersCountMethod(new StaticMethod(builder.GetMethod("BuildWithArgumentsForConstructor", bindings)), paramInfos);

            return new StaticMethod(builder.GetMethod("Build", bindings));
        }

        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private static class SubstituteBuilder<T>
            where T : class
        {
            internal static T Build()
            {
                return Substitute.For<T>();
            }

            internal static T BuildWithArgumentsForConstructor(object[] argumentsForConstructor)
            {
                return Substitute.For<T>(argumentsForConstructor);
            }
        }
    }
}