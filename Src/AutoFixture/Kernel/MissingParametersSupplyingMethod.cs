using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Decorates another method invoking it supplying missing parameters.
    /// </summary>
    public class MissingParametersSupplyingMethod : IMethod, IEquatable<MissingParametersSupplyingMethod>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingParametersSupplyingMethod"/> class.
        /// </summary>
        public MissingParametersSupplyingMethod(IMethod method)
        {
            this.Method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets the decorated method.
        /// </summary>
        public IMethod Method { get; }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters => this.Method.Parameters;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MissingParametersSupplyingMethod other && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Method.GetHashCode()
                 ^ this.Parameters.Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
        }

        /// <inheritdoc />
        public bool Equals(MissingParametersSupplyingMethod other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Method.Equals(other.Method)
                && this.Parameters.SequenceEqual(other.Parameters);
        }

        private static IEnumerable<object> GetArguments(IEnumerable<ParameterInfo> parameters, object[] arguments)
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

            return parameter.ParameterType.GetTypeInfo().IsValueType ?
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
            var arguments = GetArguments(this.Method.Parameters, parameters.ToArray());
            return this.Method.Invoke(arguments);
        }
    }
}