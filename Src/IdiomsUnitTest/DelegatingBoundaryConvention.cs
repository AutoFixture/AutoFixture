using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingBoundaryConvention : IBoundaryConvention
    {
        public DelegatingBoundaryConvention()
        {
            this.OnCreateBoundaryBehaviors = t => new[] { new DelegatingBoundaryBehavior() };
        }

        public Func<Type, IEnumerable<IBoundaryBehavior>> OnCreateBoundaryBehaviors { get; set; }

        #region IBoundaryConvention Members

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            return this.OnCreateBoundaryBehaviors(type);
        }

        #endregion
    }
}
