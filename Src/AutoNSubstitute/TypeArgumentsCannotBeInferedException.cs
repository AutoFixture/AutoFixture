using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// The exception that is thrown when AutoFixture is unable to infer the type parameters of a generic method from its arguments.
    /// </summary>
    [Serializable]
    public class TypeArgumentsCannotBeInferedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferedException"/> class from a <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> that cannot have its type arguments infered.</param>
        public TypeArgumentsCannotBeInferedException(MethodInfo methodInfo)
            : base(
                string.Format(
                    CultureInfo.CurrentCulture,
                    @"The type arguments for method '{0} {1}.{2}<{3}>({4})' cannot be infered from arguments. Make sure all type arguments can be infered (i.e. there's no type argument that is used for return type only) and you provided the right arguments to the method.",
                    GetFriendlyName(methodInfo.ReturnType),
                    methodInfo.DeclaringType.FullName,
                    methodInfo.Name,
                    string.Join(", ", methodInfo.GetGenericArguments().Select(a => a.ToString())),
                    string.Join(", ", methodInfo.GetParameters().Select(p => GetFriendlyName(p.ParameterType)))
                    ))
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferedException"/> class with a
        /// custom <see cref="Exception.Message"/>.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public TypeArgumentsCannotBeInferedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferedException"/> class with a
        /// custom <see cref="Exception.Message"/> and <see cref="Exception.InnerException"/>.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public TypeArgumentsCannotBeInferedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Ininitalizes a new instance of the <see cref="TypeArgumentsCannotBeInferedException"/> class with
        /// serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">
        /// The contextual information about the source or destination.
        /// </param>
        protected TypeArgumentsCannotBeInferedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private static string GetFriendlyName(Type type)
        {
            if (type.IsGenericType)
                return string.Format("{0}<{1}>", type.Name.Split('`')[0], string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)));

            return type.Name;
        }

    }
}