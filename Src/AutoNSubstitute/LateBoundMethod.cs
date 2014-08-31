using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.AutoNSubstitute.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
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

            return parameter.ParameterType.GetDefault();
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            var arguments = GetArguments(Method.Parameters, parameters.ToArray());
            return Method.Invoke(arguments);
        }
    }
}