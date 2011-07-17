using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingExpansion : IExpansion
    {
        public DelegatingExpansion()
        {
            this.OnExpand = v => new[] { v };
        }

        public Func<object, IEnumerable<object>> OnExpand { get; set; }

        #region IExpansion Members

        public IEnumerable<object> Expand(object value)
        {
            return this.OnExpand(value);
        }

        #endregion
    }
}
