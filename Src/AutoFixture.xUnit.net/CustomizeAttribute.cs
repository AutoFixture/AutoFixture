using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Xunit
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class CustomizeAttribute : Attribute
    {
        protected CustomizeAttribute()
        {
        }

        public abstract ICustomization GetCustomization(ParameterInfo parameter);
    }
}
