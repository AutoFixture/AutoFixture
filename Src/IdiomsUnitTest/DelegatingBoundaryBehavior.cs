using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingBoundaryBehavior : IBoundaryBehavior
    {
        public DelegatingBoundaryBehavior()
        {
            this.OnAssert = a => { };
        }

        public Action<Action<object>> OnAssert { get; set; }

        #region IBoundaryBehavior Members

        public void Assert(Action<object> action)
        {
            this.OnAssert(action);
        }

        #endregion
    }
}
