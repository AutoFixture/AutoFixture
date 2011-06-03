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
        private readonly object value;

        public PropertySetCommand(PropertyInfo propertyInfo, object owner, object value)
        {
            this.propertyInfo = propertyInfo;
            this.owner = owner;
            this.value = value;
        }

        public object Owner
        {
            get { return this.owner; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        public object Value
        {
            get { return this.value; }
        }

        #region IContextualCommand Members

        public Type ContextType
        {
            get { return default(Type); }
            set { }
        }

        public void Execute()
        {
            this.PropertyInfo.SetValue(this.Owner, this.Value, null);
        }

        #endregion
    }
}
