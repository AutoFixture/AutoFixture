using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Implements a Composite <see cref="IBoundaryConvention"/>.
    /// </summary>
    public class CompositeBoundaryConvention : IBoundaryConvention
    {
        private readonly IEnumerable<IBoundaryConvention> conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBoundaryConvention"/> class.
        /// </summary>
        /// <param name="conventions">The conventions to aggregate.</param>
        /// <seealso cref="Conventions"/>
        public CompositeBoundaryConvention(IEnumerable<IBoundaryConvention> conventions)
            : this(conventions.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBoundaryConvention"/> class.
        /// </summary>
        /// <param name="conventions">The conventions to aggregate.</param>
        /// <seealso cref="Conventions"/>
        public CompositeBoundaryConvention(params IBoundaryConvention[] conventions)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            this.conventions = conventions;
        }

        /// <summary>
        /// Gets the conventions supplied to the instance through one of the class' constructors.
        /// </summary>
        public IEnumerable<IBoundaryConvention> Conventions
        {
            get { return conventions; }
        }

        #region IBoundaryConvention Members

        /// <summary>
        /// Creates <see cref="IBoundaryBehavior"/> instances for a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A sequence of <see cref="IBoundaryBehavior"/> instances that match
        /// <paramref name="type"/>.
        /// </returns>
        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            return from c in this.conventions
                   from b in c.CreateBoundaryBehaviors(type)
                   select b;
        }

        #endregion
    }
}
