using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit.org;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    internal class DelegatingCustomizeAttribute : CustomizeAttribute
    {
        internal DelegatingCustomizeAttribute()
        {
            OnGetCustomization = p => new DelegatingCustomization();
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return OnGetCustomization(parameter);
        }

        internal Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
    }
}
