using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit
{
    public class FavorEnumerableAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return new ConstructorCustomization(parameter.ParameterType, new EnumerableFavoringConstructorQuery());
        }
    }
}
