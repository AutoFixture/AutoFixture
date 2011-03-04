using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositePropertyContext : IPropertyContext
    {
        private readonly IEnumerable<IPropertyContext> propertyContexts;

        public CompositePropertyContext(params IPropertyContext[] propertyContexts)
        {
            if (propertyContexts == null)
            {
                throw new ArgumentNullException("propertyContexts");
            }

            this.propertyContexts = propertyContexts;
        }

        public CompositePropertyContext(IEnumerable<IPropertyContext> propertyContexts)
            : this(propertyContexts.ToArray())
        {
        }

        public IEnumerable<IPropertyContext> PropertyContexts
        {
            get { return this.propertyContexts; }
        }

        #region IPropertyContext Members

        public void VerifyWritable()
        {
            foreach (var ctx in this.propertyContexts)
            {
                ctx.VerifyWritable();
            }
        }

        #endregion

        #region IMemberContext Members

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
