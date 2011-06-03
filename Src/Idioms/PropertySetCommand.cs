using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class PropertySetCommand : IGuardClauseCommand
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

        #region IContextualCommand Members

        public MemberInfo MemberInfo
        {
            get { return this.propertyInfo; }
        }

        public Type ContextType
        {
            get { return this.propertyInfo.PropertyType; }
        }

        public void Execute(object value)
        {
            this.propertyInfo.SetValue(this.Owner, value, null);
        }

        public Exception Throw(string value)
        {
            return new GuardClauseException(this.propertyInfo, this.ContextType, value);
        }

        public Exception Throw(string value, Exception innerException)
        {
            return new GuardClauseException(this.propertyInfo, this.ContextType, value, innerException);
        }

        #endregion
    }
}
