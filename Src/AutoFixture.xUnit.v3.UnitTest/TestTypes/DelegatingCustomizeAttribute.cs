using System;
using System.Reflection;

namespace AutoFixture.Xunit.v3.UnitTest.TestTypes
{
    internal class DelegatingCustomizeAttribute : CustomizeAttribute
    {
        internal DelegatingCustomizeAttribute()
        {
            this.OnGetCustomization = p => new DelegatingCustomization();
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return this.OnGetCustomization(parameter);
        }

        internal Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
    }
}