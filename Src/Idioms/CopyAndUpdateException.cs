using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about a method that incorrectly implements the idiom tested
    /// by the <see cref="CopyAndUpdateAssertion"/>.
    /// </summary>
    [Serializable]
    public class CopyAndUpdateException : Exception
    {
        [NonSerialized]
        private readonly MethodInfo methodInfo;

        [NonSerialized]
        private readonly MemberInfo memberWithInvalidValue;

        [SuppressMessage("Performance", "CA1823:Avoid unused private fields",
            Justification = "False positive - request property is used. Bug: https://github.com/dotnet/roslyn-analyzers/issues/1321")]
        [NonSerialized]
        private ParameterInfo argumentWithNoMatchingPublicMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        public CopyAndUpdateException()
            : base("The 'copy and update' method is ill-behaved.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="methodInfo">The 'copy and update' method which was ill-behaved.</param>
        public CopyAndUpdateException(string message, MethodInfo methodInfo)
            : this(message)
        {
            this.methodInfo = methodInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="methodInfo">The 'copy and update' method which was ill-behaved.</param>
        /// <param name="memberWithInvalidValue">The member which has the invalid value after copy and update.</param>
        public CopyAndUpdateException(MethodInfo methodInfo, MemberInfo memberWithInvalidValue)
            : this(FormatMessageForMethodAndMember(methodInfo, memberWithInvalidValue), methodInfo)
        {
            this.memberWithInvalidValue = memberWithInvalidValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="methodInfo">The 'copy and update' method which was ill-behaved.</param>
        /// <param name="argumentWithNoMatchingPublicMember">The parameter which has no matching public member.</param>
        public CopyAndUpdateException(MethodInfo methodInfo, ParameterInfo argumentWithNoMatchingPublicMember)
            : this(FormatMessageForMethodAndArgument(methodInfo, argumentWithNoMatchingPublicMember), methodInfo)
        {
            this.ArgumentWithNoMatchingPublicMember = argumentWithNoMatchingPublicMember;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public CopyAndUpdateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CopyAndUpdateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected CopyAndUpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the 'copy and update' method which is ill-behaved.
        /// </summary>
        public MethodInfo MethodInfo => this.methodInfo;

        /// <summary>
        /// Gets the member which was found to have an incorrect value.
        /// </summary>
        public MemberInfo MemberWithInvalidValue => this.memberWithInvalidValue;

        /// <summary>
        /// Gets the argument of the 'copy and update' method for which no matching public
        /// member could be found.
        /// </summary>
        public ParameterInfo ArgumentWithNoMatchingPublicMember
        {
            get => this.argumentWithNoMatchingPublicMember;
            set => this.argumentWithNoMatchingPublicMember = value;
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
            info.AddValue("methodInfo", this.MethodInfo);
            info.AddValue("memberWithInvalidValue", this.MemberWithInvalidValue);
            info.AddValue("argumentWithNoMatchingPublicMember", this.ArgumentWithNoMatchingPublicMember);
        }

        private static string FormatMessageForMethodAndArgument(MethodInfo methodInfo, ParameterInfo argumentWithNoMatchingPublicMember)
        {
            return string.Format(CultureInfo.CurrentCulture,
                "The method {0} failed a test for having idiomatic copy and update behaviour. " +
                "No matching public member could be found for the parameter '{1}'.{4}Declaring type: {2}{4}Reflected type: {3}{4}",
                methodInfo,
                argumentWithNoMatchingPublicMember,
                methodInfo.DeclaringType.AssemblyQualifiedName,
                methodInfo.ReflectedType.AssemblyQualifiedName,
                Environment.NewLine);
        }

        private static string FormatMessageForMethodAndMember(MethodInfo methodInfo, MemberInfo memberWithInvalidValue)
        {
            return string.Format(CultureInfo.CurrentCulture,
                "The method {0} failed a test for having idiomatic copy and update behaviour. " +
                "After execution, the member '{1}' did not have the expected value.{4}Declaring type: {2}{4}Reflected type: {3}{4}",
                methodInfo,
                memberWithInvalidValue.Name,
                methodInfo.DeclaringType.AssemblyQualifiedName,
                methodInfo.ReflectedType.AssemblyQualifiedName,
                Environment.NewLine);
        }
    }
}
