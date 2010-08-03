using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    internal class DelegatingCustomization : ICustomization
    {
        internal DelegatingCustomization()
        {
            this.OnCustomize = f => { };
        }

        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            this.OnCustomize(fixture);
        }

        #endregion

        internal Action<IFixture> OnCustomize { get; set; }
    }
}
