using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// An implementation of IConstructorQuery that composes other IConstructorQuery instances.
    /// </summary>
    [Obsolete("Use CompositeMethodQuery instead.", true)]
    public class CompositeConstructorQuery : IConstructorQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeConstructorQuery"/> class.
        /// </summary>
        /// <param name="queries">The queries.</param>
        public CompositeConstructorQuery(IEnumerable<IConstructorQuery> queries)
            : this(queries.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeConstructorQuery"/> class.
        /// </summary>
        /// <param name="queries">The queries.</param>
        public CompositeConstructorQuery(params IConstructorQuery[] queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            this.Queries = queries;
        }

        /// <summary>
        /// Gets the child builders.
        /// </summary>
        public IEnumerable<IConstructorQuery> Queries { get; }

        /// <summary>
        /// Selects the constructors for the supplied type by delegating to <see cref="Queries"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public constructors for <paramref name="type"/>, ordered by the order of the 
        /// IConstructorQuery instances in <see cref="Queries"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned constructors is based on the order of the IConstructorQuery
        /// instances in <see cref="Queries"/>.
        /// </para>
        /// <para>
        /// In case of two constructors with an equal number of parameters, the ordering is
        /// unspecified.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return (from query in this.Queries
                    from result in query.SelectConstructors(type)
                    select result);
        }
    }
}
