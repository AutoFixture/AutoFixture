using System;
using Ploeh.AutoFixture;

namespace Ploe.AutoFixture.NUnit.UnitTest
{
    internal class DelegatingCustomization : ICustomization
    {
        internal DelegatingCustomization()
        {
            OnCustomize = f => { };
        }

        public void Customize(IFixture fixture)
        {
            OnCustomize(fixture);
        }

        internal Action<IFixture> OnCustomize { get; set; }
    }
}
