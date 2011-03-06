using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// A Composite of <see cref="IMemberContext"/> instances.
    /// </summary>
    public class CompositeMemberContext : IMemberContext
    {
        private readonly IEnumerable<IMemberContext> memberContexts;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMemberContext"/> class.
        /// </summary>
        /// <param name="memberContexts">The member contexts to aggregate.</param>
        public CompositeMemberContext(params IMemberContext[] memberContexts)
        {
            if (memberContexts == null)
            {
                throw new ArgumentNullException("memberContexts");
            }

            this.memberContexts = memberContexts;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMemberContext"/> class.
        /// </summary>
        /// <param name="memberContexts">The member contexts to aggregate.</param>
        public CompositeMemberContext(IEnumerable<IMemberContext> memberContexts)
            : this(memberContexts.ToArray())
        {
        }

        /// <summary>
        /// Gets the member contexts provided to the constructor of the current instance.
        /// </summary>
        public IEnumerable<IMemberContext> MemberContexts
        {
            get { return this.memberContexts; }
        }

        #region IMemberContext Members

        /// <summary>
        /// Verifies the boundaries conditions of the type member(s) aggregated by the context.
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

            foreach (var ctx in this.memberContexts)
            {
                ctx.VerifyBoundaries(convention);
            }
        }

        #endregion
    }
}
