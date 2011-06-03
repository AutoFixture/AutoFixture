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
        private readonly object owner;

        public PropertySetCommand(PropertyInfo propertyInfo, object owner)
        {
            this.propertyInfo = propertyInfo;
            this.owner = owner;
        }

        public object Owner
        {
            get { return this.owner; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        #region IContextualCommand Members

        public MemberInfo MemberInfo
        {
            get { return null; }
        }

        public Type ContextType
        {
            get { return this.PropertyInfo.PropertyType; }
        }

        public void Execute(object value)
        {
            this.PropertyInfo.SetValue(this.Owner, value, null);
        }

        #endregion
    }
}
