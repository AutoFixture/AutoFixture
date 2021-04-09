using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A query which returns generic collection properties from specified types.
    /// </summary>
    public class GenericCollectionPropertyQuery : IPropertyQuery
    {
        /// <summary>
        /// Select those properties that are generic collections from <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type which generic collection properties should be selected from.</param>
        /// <returns>Properties belonging to <paramref name="type"/> that are collections.</returns>
        public IEnumerable<PropertyInfo> SelectProperties(Type type)
        {
            return type.GetTypeInfo().GetProperties()
                .Where(p =>
                    p.PropertyType.Name == typeof(ICollection<>).Name ||
                    p.PropertyType.GetTypeInfo().GetInterface(typeof(ICollection<>).Name) != null);
        }
    }
}