using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class DelegatingCustomization : ICustomization
    {
        public DelegatingCustomization()
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
