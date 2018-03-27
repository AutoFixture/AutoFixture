using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoMoq.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Gets a collection of all methods declared by the interface <paramref name="type"/> or any of its base interfaces.
        /// </summary>
        /// <param name="type">An interface type.</param>
        /// <returns>A collection of all methods declared by the interface <paramref name="type"/> or any of its base interfaces.</returns>
        internal static IEnumerable<MethodInfo> GetInterfaceMethods(this Type type)
        {
            return type.GetMethods().Concat(
                type.GetInterfaces()
                    .SelectMany(@interface => @interface.GetMethods()));
        }

        /// <summary>
        /// Gets a collection of all properties declared by the interface <paramref name="type"/> or any of its base interfaces.
        /// </summary>
        /// <param name="type">An interface type.</param>
        /// <returns>A collection of all properties declared by the interface <paramref name="type"/> or any of its base interfaces.</returns>
        internal static IEnumerable<PropertyInfo> GetInterfaceProperties(this Type type)
        {
            return type.GetProperties().Concat(
                type.GetInterfaces()
                    .SelectMany(@interface => @interface.GetProperties()));
        }
    }
}
