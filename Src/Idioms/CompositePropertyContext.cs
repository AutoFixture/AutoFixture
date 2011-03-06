using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// A Composite of <see cref="IPropertyContext"/> instances.
    /// </summary>
    public class CompositePropertyContext : IPropertyContext
    {
        private readonly IEnumerable<IPropertyContext> propertyContexts;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePropertyContext"/> class.
        /// </summary>
        /// <param name="propertyContexts">The property contexts to aggregate.</param>
        public CompositePropertyContext(params IPropertyContext[] propertyContexts)
        {
            if (propertyContexts == null)
            {
                throw new ArgumentNullException("propertyContexts");
            }

            this.propertyContexts = propertyContexts;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePropertyContext"/> class.
        /// </summary>
        /// <param name="propertyContexts">The property contexts to aggregate.</param>
        public CompositePropertyContext(IEnumerable<IPropertyContext> propertyContexts)
            : this(propertyContexts.ToArray())
        {
        }

        /// <summary>
        /// Gets the property contexts provided to the constructor of the instance.
        /// </summary>
        public IEnumerable<IPropertyContext> PropertyContexts
        {
            get { return this.propertyContexts; }
        }

        #region IPropertyContext Members

        /// <summary>
        /// Verifies that the property or properties aggregated by the context is or are
        /// writable, and that the value returned is the same as the value which was originally
        /// assigned.
        /// </summary>
        public void VerifyWritable()
        {
            foreach (var ctx in this.propertyContexts)
            {
                ctx.VerifyWritable();
            }
        }

        #endregion

        #region IMemberContext Members

        /// <summary>
        /// Verifies the boundaries conditions of the property or properties aggregated by the
        /// context.
        /// </summary>
        /// <param name="convention">The convention to use to verify the boundaries.</param>
        /// <remarks>
        /// An example of a convention could be to verify that all properties are protected
        /// by Guard Clauses the protect against null references.
        /// </remarks>
        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            foreach (var ctx in this.propertyContexts)
            {
                ctx.VerifyBoundaries(convention);
            }
        }

        #endregion
    }
}
