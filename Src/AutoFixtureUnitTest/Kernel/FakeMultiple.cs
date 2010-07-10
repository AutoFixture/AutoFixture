using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FakeMultiple : IMultiple
    {
        #region IMultiple Members

        public int Count { get; set; }

        #endregion
    }
}
