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
            this.OnCreateBoundaryBehaviors = f => new[] { new DelegatingBoundaryBehavior() };
        }

        public Func<IFixture, IEnumerable<IBoundaryBehavior>> OnCreateBoundaryBehaviors { get; set; }

        #region IBoundaryConvention Members

        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(IFixture fixture)
        {
            return this.OnCreateBoundaryBehaviors(fixture);
        }

        #endregion
    }
}
