using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains extension methods for <see cref="System.Type"/> class.
    /// </summary>
    public static class TypeFilter
    {
        /// <summary>
        /// Checks if type is a struct. This will exclude primitive types (int, char etc.) considere as <see cref="Type.IsPrimitive"/> 
        /// but not .net structs. 
        /// </summary>
        /// <param name="type">Type that needs to be checked.</param>
        /// <returns>
        /// <see langword="true"/> if given type is a custom created struct, <see langword="false" />.
        /// </returns>
        public static bool IsStructure(this Type type)
        {
            return type.IsValueType && !type.IsEnum && !type.IsPrimitive;
        }
    }
}