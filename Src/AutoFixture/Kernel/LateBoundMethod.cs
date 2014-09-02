using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Decorates another method invoking it supplying missing optional parameters
    /// </summary>
    public class LateBoundMethod : IMethod
    {
        private readonly IMethod _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="LateBoundMethod"/> class.
        /// </summary>
        /// <param name="method">The <see cref="IMethod"/> to decorate.</param>
        public LateBoundMethod(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            this._method = method;
        }

        /// <summary>
        /// Gets the decorated method
        /// </summary>
        public IMethod Method
        {
            get { return this._method; }
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return Method.Parameters; }
        }

        private static IEnumerable<object> GetArguments(IEnumerable<ParameterInfo> parameters, object[] arguments)
        {
            return parameters.Select((p, i) => arguments.Length > i ? arguments[i] : GetDefault(p));
        }

        private static object GetDefault(ParameterInfo parameter)
        {
            if (parameter.IsOptional)
                return parameter.DefaultValue;

            if (parameter.IsDefined(typeof(ParamArrayAttribute), true) && parameter.ParameterType.IsArray)
                return Array.CreateInstance(parameter.ParameterType.GetElementType(), 0);

            return parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null;
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