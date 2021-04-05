using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A query which is used to compose multiple <see cref="IPropertyQuery"/>s, returning only those properties
    /// that are returned by all queries.
    /// </summary>
    public class AndPropertyQuery : IPropertyQuery
    {
        /// <summary>
        /// Constructs an instance of <see cref="AndPropertyQuery"/>, that will select properties from specified types
        /// that are returned by <paramref name="queries"/>.
        /// </summary>
        /// <param name="queries">The queries that should be used to select properties.</param>
        public AndPropertyQuery(params IPropertyQuery[] queries)
        {
            this.Queries = queries;
        }

        /// <summary>
        /// Gets the queries that are used to select properties from specified types.
        /// </summary>
        public IEnumerable<IPropertyQuery> Queries { get; }

        /// <summary>
        /// Selects properties from <paramref name="type"/> that are returned by <see cref="Queries"/>.
        /// </summary>
        /// <param name="type">The type which properties should be selected from.</param>
        /// <returns>Properties belonging to <paramref name="type"/> that meet <see cref="Queries"/>.</returns>
        public IEnumerable<PropertyInfo> SelectProperties(Type type)
        {
            var properties = new HashSet<PropertyInfo>(this.Queries.First().SelectProperties(type));

            foreach (var query in this.Queries.Skip(1))
            {
                properties.IntersectWith(query.SelectProperties(type));
            }

            return properties;
        }
    }
}