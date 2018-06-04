using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

#if SYSTEM_RUNTIME_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace AutoFixture.Kernel
{
    /// <summary>
    /// The exception that is thrown when AutoFixture is unable to infer the type
    /// parameters of a generic method from its arguments.
    /// </summary>
#if SYSTEM_RUNTIME_SERIALIZATION
    [Serializable]
#endif
    public class TypeArgumentsCannotBeInferredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferredException"/> class.
        /// </summary>
        public TypeArgumentsCannotBeInferredException()
            : base("The type arguments for the method cannot be inferred from arguments. Make sure all type arguments can be inferred (i.e. there's no type argument that is used for return type only) and you provided the right arguments to the method.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferredException"/> class
        /// from a <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">
        /// The <see cref="MethodInfo"/> that cannot have its type arguments inferred.
        /// </param>
        public TypeArgumentsCannotBeInferredException(MethodInfo methodInfo)
            : base(methodInfo == null ? string.Empty :
                string.Format(
                    CultureInfo.CurrentCulture,
                    @"The type arguments for method '{0} {1}.{2}<{3}>({4})' cannot be inferred from arguments. Make sure all type arguments can be inferred (i.e. there's no type argument that is used for return type only) and you provided the right arguments to the method.",
                    GetFriendlyName(methodInfo.ReturnType),
                    methodInfo.DeclaringType.FullName,
                    methodInfo.Name,
                    string.Join(", ", methodInfo.GetGenericArguments().Select(a => a.ToString())),
                    string.Join(", ", methodInfo.GetParameters().Select(p => GetFriendlyName(p.ParameterType)))))
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferredException"/> class with a
        /// custom <see cref="Exception.Message"/>.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public TypeArgumentsCannotBeInferredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferredException"/> class with a
        /// custom <see cref="Exception.Message"/> and <see cref="Exception.InnerException"/>.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public TypeArgumentsCannotBeInferredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if SYSTEM_RUNTIME_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentsCannotBeInferredException"/> class with
        /// serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">
        /// The contextual information about the source or destination.
        /// </param>
        protected TypeArgumentsCannotBeInferredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        private static string GetFriendlyName(Type type)
        {
            if (type.GetTypeInfo().IsGenericType)
            {
                return string.Format(CultureInfo.CurrentCulture,
                    "{0}<{1}>",
                    type.Name.Split('`')[0],
                    string.Join(", ", type.GetTypeInfo().GetGenericArguments().Select(GetFriendlyName)));
            }

            return type.Name;
        }
    }
}