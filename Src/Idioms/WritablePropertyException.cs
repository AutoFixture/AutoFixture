using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    [Serializable]
    public class WritablePropertyException : Exception
    {
        private readonly PropertyInfo propertyInfo;

        public WritablePropertyException(PropertyInfo propertyInfo)
            : this(propertyInfo, string.Format("The property {0} failed a test for being well-behaved writable. The getter does not return the value assigned to the setter.{3}Declaring type: {1}{3}Reflected type: {2}{3}", propertyInfo, propertyInfo.DeclaringType.AssemblyQualifiedName, propertyInfo.ReflectedType.AssemblyQualifiedName, Environment.NewLine))
        {
        }

        public WritablePropertyException(PropertyInfo propertyInfo, string message)
            : base(message)
        {
            this.propertyInfo = propertyInfo;
        }

        public WritablePropertyException(PropertyInfo propertyInfo, string message, Exception innerException)
            : base(message, innerException)
        {
            this.propertyInfo = propertyInfo;
        }

        protected WritablePropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }
    }
}
