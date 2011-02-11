using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeMethodContext : IMethodContext
    {
        private readonly IEnumerable<IMethodContext> methodContexts;

        public CompositeMethodContext(params IMethodContext[] methodContexts)
        {
            if (methodContexts == null)
            {
                throw new ArgumentNullException("methodContexts");
            }

            this.methodContexts = methodContexts;
        }

        public CompositeMethodContext(IEnumerable<IMethodContext> methodContexts)
            : this(methodContexts.ToArray())
        {
        }

        public IEnumerable<IMethodContext> MethodContexts
        {
            get { return this.methodContexts; }
        }

        #region IMemberContext Members

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
