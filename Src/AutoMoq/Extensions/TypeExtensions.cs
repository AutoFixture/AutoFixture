using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoMoq.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Gets a collection of all methods declared by the type or any of its base interfaces.
        /// </summary>
        internal static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            IEnumerable<MethodInfo> result = type.GetMethods();
            
            // If "type" is an interface, "GetMethods" does not return methods declared on other interfaces extended by "type".
            if (type.GetTypeInfo().IsInterface)
                result = result.Concat(type.GetInterfaces().SelectMany(x => x.GetMethods()));

            return result;
        }

        /// <summary>
        /// Gets a collection of all properties declared by the type or any of its base interfaces.
        /// </summary>
        internal static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            IEnumerable<PropertyInfo> result = type.GetProperties();
            
            // If "type" is an interface, "GetProperties" does not return methods declared on other interfaces extended by "type".
            if (type.GetTypeInfo().IsInterface)
                result = result.Concat(type.GetInterfaces().SelectMany(x => x.GetProperties()));

            return result;
        }        
    }
}
