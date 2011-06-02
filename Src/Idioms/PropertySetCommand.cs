using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class PropertySetCommand : IContextualCommand
    {
        private readonly PropertyInfo propertyInfo;

        public PropertySetCommand(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public System.Reflection.PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }
    }
}
