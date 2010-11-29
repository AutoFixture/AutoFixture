using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingBoundaryConventionFactory : IBoundaryConventionFactory
    {
        public DelegatingBoundaryConventionFactory()
        {
            this.OnGetConvention = t => new DelegatingBoundaryConvention();
        }

        public Func<Type, IBoundaryConvention> OnGetConvention { get; set; }

        #region IBoundaryConventionFactory Members

        public IBoundaryConvention GetConvention(Type type)
        {
            return this.OnGetConvention(type);
        }

        #endregion
    }
}
