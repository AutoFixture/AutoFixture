using System;
using System.Linq;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal static class ProxyType
    {
        internal static ConstructorInfo GetModestConstructor(this Type type)
        {
            return (from ci in type.GetConstructors(
                     BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                    orderby ci.GetParameters().Length ascending
                    select ci).First();
        }

        internal static PropertyInfo MatchProperty(this Type type, string name)
        {
            return type.GetProperty(name)
                ?? type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .First(x => x.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
