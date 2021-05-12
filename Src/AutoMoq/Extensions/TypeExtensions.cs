using System;
using System.Reflection;

namespace AutoFixture.AutoMoq.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Returns whether or not a type represents a delegate.
        /// </summary>
        internal static bool IsDelegate(this Type type)
        {
            return typeof(MulticastDelegate).IsAssignableFrom(type.GetTypeInfo().BaseType);
        }
    }
}
