using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents a verification error when testing whether a writable property is correctly
    /// implemented.
    /// </summary>
    [Serializable]
    public class WritablePropertyException : Exception
    {
        private readonly PropertyInfo propertyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritablePropertyException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        public WritablePropertyException(PropertyInfo propertyInfo)
            : this(propertyInfo, WritablePropertyException.FormatDefaultMessage(propertyInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritablePropertyException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public WritablePropertyException(PropertyInfo propertyInfo, string message)
            : base(message)
        {
            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritablePropertyException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public WritablePropertyException(PropertyInfo propertyInfo, string message, Exception innerException)
            : base(message, innerException)
        {
            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritablePropertyException"/> class with
        /// serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected WritablePropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the property supplied via the constructor.
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        private static string FormatDefaultMessage(PropertyInfo propertyInfo)
        {
            return string.Format("The property {0} failed a test for being well-behaved writable. The getter does not return the value assigned to the setter.{3}Declaring type: {1}{3}Reflected type: {2}{3}", propertyInfo, propertyInfo.DeclaringType.AssemblyQualifiedName, propertyInfo.ReflectedType.AssemblyQualifiedName, Environment.NewLine);
        }
    }
}
