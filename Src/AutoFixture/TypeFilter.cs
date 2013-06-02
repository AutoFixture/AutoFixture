﻿using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains extension methods for <see cref="System.Type"/> class.
    /// </summary>
    internal static class TypeFilter
    {
        /// <summary>
        /// Checks if type is a struct. This will exclude primitive types (int, char etc.) considered as <see cref="Type.IsPrimitive"/> as well as enums
        /// but not .net structs. 
        /// </summary>
        /// <param name="type">Type that needs to be checked.</param>
        /// <returns>
        /// <see langword="true"/> if given type is a value type (but not enum or primitive type), <see langword="false" />.
        /// </returns>
        public static bool IsValueTypeButNotPrimitiveOrEnum(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.IsValueType && !type.IsEnum && !type.IsPrimitive;
        }
    }
}