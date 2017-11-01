using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// An implementation of <see cref="IMethodQuery"/> that composes other IMethodQuery instances.
    /// </summary>
    public class CompositeMethodQuery : IMethodQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMethodQuery"/> class.
        /// </summary>
        /// <param name="queries">The queries.</param>
        public CompositeMethodQuery(IEnumerable<IMethodQuery> queries)
            : this(queries.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMethodQuery"/> class.
        /// </summary>
        /// <param name="queries">The queries.</param>
        public CompositeMethodQuery(params IMethodQuery[] queries)
        {
            this.Queries = queries ?? throw new ArgumentNullException(nameof(queries));
        }

        /// <summary>
        /// Gets the queries supplied through one of the constructors.
        /// </summary>
        public IEnumerable<IMethodQuery> Queries { get; }

        /// <summary>
        /// Selects the methods for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return from q in this.Queries
                   from m in q.SelectMethods(type)
                   select m;
        }
    }
}
