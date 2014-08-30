using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.AutoNSubstitute.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal class LateBoundMethod : IMethod
    {
        private readonly IMethod _method;

        public LateBoundMethod(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            this._method = method;
        }

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
            return parameters.Select((p, i) =>
                arguments.Length > i ? 
                arguments[i] : 
                p.IsOptional ? 
                    p.DefaultValue : 
                    p.ParameterType.GetDefault());
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            var arguments = GetArguments(Method.Parameters, parameters.ToArray());
            return Method.Invoke(arguments);
        }
    }
}