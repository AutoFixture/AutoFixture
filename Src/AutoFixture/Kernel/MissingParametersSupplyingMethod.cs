using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Decorates another method invoking it supplying missing parameters
    /// </summary>
    public class MissingParametersSupplyingMethod : IMethod, IEquatable<MissingParametersSupplyingMethod>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingParametersSupplyingMethod"/> class.
        /// </summary>
        /// <param name="method">The <see cref="IMethod"/> to decorate.</param>
        public MissingParametersSupplyingMethod(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            this.Method = method;
        }

        /// <summary>
        /// Gets the decorated method
        /// </summary>
        public IMethod Method { get; }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return Method.Parameters; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            var other = obj as MissingParametersSupplyingMethod;
            if (other != null)
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
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MissingParametersSupplyingMethod other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Method.Equals(other.Method)
                && this.Parameters.SequenceEqual(other.Parameters);
        }

        private static IEnumerable<object> GetArguments(IEnumerable<ParameterInfo> parameters, 
            object[] arguments)
        {
            return parameters.Select((p, i) => arguments.Length > i ? arguments[i] : GetDefault(p));
        }

        private static object GetDefault(ParameterInfo parameter)
        {
            if (parameter.IsOptional)
                return parameter.DefaultValue;

            if (parameter.IsDefined(typeof(ParamArrayAttribute), true) && 
                parameter.ParameterType.IsArray)
                return Array.CreateInstance(parameter.ParameterType.GetElementType(), 0);

            return parameter.ParameterType.IsValueType() ? 
                Activator.CreateInstance(parameter.ParameterType) : 
                null;
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            var arguments = GetArguments(Method.Parameters, parameters.ToArray());
            return Method.Invoke(arguments);
        }
    }
}