using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// A Composite of <see cref="IMethodContext"/> instances.
    /// </summary>
    public class CompositeMethodContext : IMethodContext
    {
        private readonly IEnumerable<IMethodContext> methodContexts;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMethodContext"/> class.
        /// </summary>
        /// <param name="methodContexts">The method contexts to aggregate.</param>
        public CompositeMethodContext(params IMethodContext[] methodContexts)
        {
            if (methodContexts == null)
            {
                throw new ArgumentNullException("methodContexts");
            }

            this.methodContexts = methodContexts;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMethodContext"/> class.
        /// </summary>
        /// <param name="methodContexts">The method contexts to aggregate.</param>
        public CompositeMethodContext(IEnumerable<IMethodContext> methodContexts)
            : this(methodContexts.ToArray())
        {
        }

        /// <summary>
        /// Gets the method contexts provided to the constructor of the current instance.
        /// </summary>
        public IEnumerable<IMethodContext> MethodContexts
        {
            get { return this.methodContexts; }
        }

        #region IMemberContext Members

        /// <summary>
        /// Verifies the boundaries conditions of the method(s) aggregated by the context.
        /// </summary>
        /// <param name="convention">The convention to use to verify the boundaries.</param>
        /// <remarks>
        /// An example of a convention could be to verify that all method parameters are protected
        /// by Guard Clauses the protect against null references.
        /// </remarks>
        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            foreach (var ctx in this.MethodContexts)
            {
                ctx.VerifyBoundaries(convention);
            }
        }

        #endregion
    }
}
