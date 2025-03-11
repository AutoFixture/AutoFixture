using System;

namespace AutoFixture.AutoMoq.UnitTest
{
    internal class DelegatingMockCustomization : ICustomization
    {
        internal DelegatingMockCustomization()
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
