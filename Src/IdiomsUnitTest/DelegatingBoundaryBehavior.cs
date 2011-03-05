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
            this.OnAssert = (a, ctx) => { };
        }

        public Action<Action<object>, string> OnAssert { get; set; }

        #region IBoundaryBehavior Members

        public void Assert(Action<object> action, string context)
        {
            this.OnAssert(action, context);
        }

        #endregion
    }
}
