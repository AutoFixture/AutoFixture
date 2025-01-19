using System;
using System.Reflection;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class DelegatingCustomizeAttribute : CustomizeAttribute
    {
        public DelegatingCustomizeAttribute()
        {
            this.OnGetCustomization = p => new DelegatingCustomization();
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return this.OnGetCustomization(parameter);
        }

        public Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
    }
}
