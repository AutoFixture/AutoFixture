using System;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class DelegatingCustomization : ICustomization
    {
        public DelegatingCustomization()
        {
            this.OnCustomize = f => { };
        }

        public void Customize(IFixture fixture)
        {
            this.OnCustomize(fixture);
        }

        internal Action<IFixture> OnCustomize { get; set; }
    }
}
