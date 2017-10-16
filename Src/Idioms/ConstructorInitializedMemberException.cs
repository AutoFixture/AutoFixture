using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Represents a verification error when testing whether a read-only property is correctly
    /// implemented.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "This exception's invariants require the propertyInfo to be present. Thus, constructors without the propertyInfo would violate the invarient. However, constructors matching the standard Exception constructors have been implemented.")]
    [Serializable]
    public class ConstructorInitializedMemberException : Exception
    {
        [NonSerialized]
        private readonly MemberInfo memberInfo;
        [NonSerialized]
        private readonly ParameterInfo missingParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="constructorInfo">The Constructor.</param>
        /// <param name="missingParameter">The parameter that was not exposed as a field or property.</param>
        public ConstructorInitializedMemberException(ConstructorInfo constructorInfo, ParameterInfo missingParameter)
            : this(constructorInfo, missingParameter,
                ConstructorInitializedMemberException.FormatDefaultMessage(constructorInfo, missingParameter))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="constructorInfo">The Constructor.</param>
        /// <param name="missingParameter">The parameter that was not exposed as a field or property.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ConstructorInitializedMemberException(ConstructorInfo constructorInfo, ParameterInfo missingParameter, string message)
            : base(message)
        {
            this.memberInfo = constructorInfo;
            this.missingParameter = missingParameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="constructorInfo">The Constructor.</param>
        /// <param name="missingParameter">The parameter that was not exposed as a field or property.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ConstructorInitializedMemberException(ConstructorInfo constructorInfo, ParameterInfo missingParameter, string message,
            Exception innerException)
            : base(message, innerException)
        {
            this.memberInfo = constructorInfo;
            this.missingParameter = missingParameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        public ConstructorInitializedMemberException(FieldInfo fieldInfo)
            : this(fieldInfo, ConstructorInitializedMemberException.FormatDefaultMessage(fieldInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ConstructorInitializedMemberException(FieldInfo fieldInfo, string message)
            : base(message)
        {
            this.memberInfo = fieldInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ConstructorInitializedMemberException(FieldInfo fieldInfo, string message, Exception innerException)
            : base(message, innerException)
        {
            this.memberInfo = fieldInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        public ConstructorInitializedMemberException(PropertyInfo propertyInfo)
            : this(propertyInfo, ConstructorInitializedMemberException.FormatDefaultMessage(propertyInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ConstructorInitializedMemberException(PropertyInfo propertyInfo, string message)
            : base(message)
        {
            this.memberInfo = propertyInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ConstructorInitializedMemberException(PropertyInfo propertyInfo, string message, Exception innerException)
            : base(message, innerException)
        {
            this.memberInfo = propertyInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInitializedMemberException"/> class with
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
        protected ConstructorInitializedMemberException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.memberInfo = (PropertyInfo)info.GetValue("memberInfo", typeof(MemberInfo));
        }

        /// <summary>
        /// Gets the property or field supplied via the constructor.
        /// </summary>
        public MemberInfo MemberInfo
        {
            get { return this.memberInfo; }
        }

        /// <summary>
        /// Gets the property supplied via the constructor.
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return this.memberInfo as PropertyInfo; }
        }

        /// <summary>
        /// Gets the property supplied via the constructor.
        /// </summary>
        public FieldInfo FieldInfo
        {
            get { return this.memberInfo as FieldInfo; }
        }

        /// <summary>
        /// Gets the constructor which has a <see cref="MissingParameter"/>.
        /// </summary>
        public ConstructorInfo ConstructorInfo
        {
            get { return this.memberInfo as ConstructorInfo; }
        }

        /// <summary>
        /// Gets the parameter that was not exposed as a field or property.
        /// </summary>
        public ParameterInfo MissingParameter
        {
            get { return this.missingParameter; }
        }

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

            info.AddValue("memberInfo", this.memberInfo);
        }

        private static string FormatDefaultMessage(ConstructorInfo constructorInfo, ParameterInfo missingParameter)
        {
            return string.Format(CultureInfo.CurrentCulture,
                "The constructor {0} failed a test for having each parameter be exposed as a well-behaved read-only property or field. " +
                "The field {1} was not exposed publicly.{4}Declaring type: {2}{4}Reflected type: {3}{4}",
                constructorInfo,
                missingParameter.Name,
                constructorInfo.DeclaringType.AssemblyQualifiedName,
                constructorInfo.ReflectedType.AssemblyQualifiedName,
                Environment.NewLine);
        }

        private static string FormatDefaultMessage(PropertyInfo propertyInfo)
        {
            return string.Format(CultureInfo.CurrentCulture, "The property {0} failed a test for being well-behaved read-only property. The getter does not return the value assigned to the constructor.{3}Declaring type: {1}{3}Reflected type: {2}{3}", propertyInfo, propertyInfo.DeclaringType.AssemblyQualifiedName, propertyInfo.ReflectedType.AssemblyQualifiedName, Environment.NewLine);
        }

        private static string FormatDefaultMessage(FieldInfo fieldInfo)
        {
            return string.Format(CultureInfo.CurrentCulture, "The property {0} failed a test for being well-behaved read-only field. The field does not return the value assigned to the constructor.{3}Declaring type: {1}{3}Reflected type: {2}{3}", fieldInfo, fieldInfo.DeclaringType.AssemblyQualifiedName, fieldInfo.ReflectedType.AssemblyQualifiedName, Environment.NewLine);
        }
    }
}
