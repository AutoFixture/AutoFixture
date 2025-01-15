using System;
using System.Reflection;

namespace AutoFixture.AutoMoq.UnitTest
{
    internal class DelegatingMockCustomizeAttribute : MockCustomizeAttribute
    {
        internal DelegatingMockCustomizeAttribute()
        {
            this.OnGetCustomization = p => new DelegatingMockCustomization();
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return this.OnGetCustomization(parameter);
        }

        internal Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
    }
}
