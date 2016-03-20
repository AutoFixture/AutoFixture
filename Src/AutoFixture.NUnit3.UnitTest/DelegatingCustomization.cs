using System;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    internal class DelegatingCustomization : ICustomization
    {
        internal DelegatingCustomization()
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
