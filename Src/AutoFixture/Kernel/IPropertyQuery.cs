using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Defines a strategy for selecting properties from a type.
    /// </summary>
    public interface IPropertyQuery
    {
        /// <summary>
        /// Selects the properties for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Property information for properties belonging to <paramref name="type"/>.</returns>
        IEnumerable<PropertyInfo> SelectProperties(Type type);
    }
}