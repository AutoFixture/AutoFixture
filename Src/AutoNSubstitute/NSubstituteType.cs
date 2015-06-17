using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal static class NSubstituteType
    {
        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static IEnumerable<Type> GetSubstituteTypes(this Type type)
        {
	        var substitute = Substitute.For<object>();
	        var interfaces = substitute.GetType().GetInterfaces();

            return type.GetInterfaces()
                .Where(i => !interfaces.Contains(i))
                .Concat(new[] {type.BaseType});
        }
    }
}