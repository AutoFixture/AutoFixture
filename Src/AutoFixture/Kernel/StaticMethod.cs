using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a static method.
    /// </summary>
    public class StaticMethod : IMethod, IEquatable<StaticMethod>
    {
        private readonly ParameterInfo[] paramInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticMethod"/> class.
        /// </summary>
        /// <param name="methodInfo">The methodInfo.</param>
        public StaticMethod(MethodInfo methodInfo)
            : this(methodInfo, GetMethodParameters(methodInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticMethod"/> class.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="methodParameters">The method parameters.</param>
        public StaticMethod(MethodInfo methodInfo, ParameterInfo[] methodParameters)
        {
            this.Method = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            this.paramInfos = methodParameters ?? throw new ArgumentNullException(nameof(methodParameters));
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters => this.paramInfos;

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj is StaticMethod other)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Method.GetHashCode()
                 ^ this.Parameters.Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            return this.Method.Invoke(null, parameters.ToArray());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(StaticMethod other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Method.Equals(other.Method)
                && this.Parameters.SequenceEqual(other.Parameters);
        }

        private static ParameterInfo[] GetMethodParameters(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            return methodInfo.GetParameters();
        }
    }
}
