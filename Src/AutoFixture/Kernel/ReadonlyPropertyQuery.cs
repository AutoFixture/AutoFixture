using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A query which returns readonly properties from specified types.
    /// </summary>
    public class ReadonlyPropertyQuery : IPropertyQuery
    {
        /// <summary>
        /// Select those properties that are readonly from <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type which readonly properties should be selected from.</param>
        /// <returns>Properties belonging to <paramref name="type"/> that are readonly.</returns>
        public IEnumerable<PropertyInfo> SelectProperties(Type type)
        {
            return type.GetTypeInfo().GetProperties().Where(p => p.GetSetMethod() == null);
        }
    }
}