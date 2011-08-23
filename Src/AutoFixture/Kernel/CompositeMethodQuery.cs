using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// An implementation of <see cref="IMethodQuery"/> that composes other IMethodQuery instances.
    /// </summary>
    public class CompositeMethodQuery : IMethodQuery
    {
        private readonly IEnumerable<IMethodQuery> queries;

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
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }
            
            this.queries = queries;
        }

        /// <summary>
        /// Gets the queries supplied through one of the constructors.
        /// </summary>
        public IEnumerable<IMethodQuery> Queries
        {
            get { return this.queries; }
        }

        /// <summary>
        /// Selects the methods for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return from q in this.queries
                   from m in q.SelectMethods(type)
                   select m;
        }
    }
}
