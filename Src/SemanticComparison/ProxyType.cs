using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal static class ProxyType
    {
        internal static ConstructorInfo GetCompatibleConstructor(this Type type)
        {
            List<Type> propertyTypes = type.GetProperties().Select(x => x.PropertyType).ToList();

            return (from ci in type.GetConstructors(
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                    let parameterTypes = ci.GetParameters().Select(x => x.ParameterType).ToList()
                    where parameterTypes.All(t => Matches(propertyTypes, t))
                    orderby parameterTypes.Count ascending
                    select ci).First();
        }

        internal static bool Matches(this List<Type> publicTypes, Type type)
        {
            return publicTypes.Contains(type)
                || publicTypes.Any(t => t.IsAssignableFrom(type)
                    || type.IsAssignableFrom(t));
        }

        internal static PropertyInfo MatchProperty(this Type type, string name)
        {
            return type.GetProperty(name)
                ?? type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .First(x => x.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
