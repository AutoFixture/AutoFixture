using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Represents a verification error when testing whether a writable property is correctly
    /// implemented.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "This exception's invariants require the propertyInfo to be present. Thus, constructors without the propertyInfo would violate the invarient. However, constructors matching the standard Exception constructors have been implemented.")]
    [Serializable]
    public class WritablePropertyException : Exception
    {
        [NonSerialized]
        private readonly PropertyInfo propertyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritablePropertyException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        public WritablePropertyException(PropertyInfo propertyInfo)
            : this(propertyInfo, FormatDefaultMessage(propertyInfo))
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
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected WritablePropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
#if SERIALIZABLE_MEMBERINFO
            this.propertyInfo = (PropertyInfo)info.GetValue("PropertyInfo", typeof(PropertyInfo));
#endif
        }

        /// <summary>
        /// Gets the property supplied via the constructor.
        /// <remarks>
        /// Notice, value might null after deserialization on platforms that don't support <see cref="PropertyInfo"/> serialization.
        /// </remarks> 
        /// </summary>
        public PropertyInfo PropertyInfo => this.propertyInfo;

        /// <summary>
        /// Adds <see cref="PropertyInfo" /> to a
        /// <see cref="System.Runtime.Serialization.SerializationInfo"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

#if SERIALIZABLE_MEMBERINFO
            info.AddValue("PropertyInfo", this.propertyInfo);
#endif
        }

        private static string FormatDefaultMessage(PropertyInfo propertyInfo)
        {
            return string.Format(CultureInfo.CurrentCulture, "The property {0} failed a test for being well-behaved writable. The getter does not return the value assigned to the setter.{3}Declaring type: {1}{3}Reflected type: {2}{3}", propertyInfo, propertyInfo.DeclaringType.AssemblyQualifiedName, propertyInfo.ReflectedType.AssemblyQualifiedName, Environment.NewLine);
        }
    }
}
