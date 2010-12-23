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
            this.OnCreateBoundaryBehaviors = () => new[] { new DelegatingBoundaryBehavior() };
        }

        public Func<IEnumerable<IBoundaryBehavior>> OnCreateBoundaryBehaviors { get; set; }

        #region IBoundaryConvention Members

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors()
        {
            return this.OnCreateBoundaryBehaviors();
        }

        #endregion
    }
}
