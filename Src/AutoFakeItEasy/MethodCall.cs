using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoFakeItEasy
{
    internal class MethodCall
    {
        private readonly string methodName;
        private readonly IList<Type> parameterTypes;
        private readonly IList<object> arguments;

        public MethodCall(string methodName, IEnumerable<ParameterInfo> parameters, IEnumerable<object> arguments)
        {
            this.methodName = methodName;
            var parametersList = parameters.ToList();
            this.parameterTypes = parametersList.Select(p => p.ParameterType).ToList();
            this.arguments = ExpandParamsArgument(parametersList, arguments).ToList();
        }

        public override bool Equals(object obj)
        {
            return obj is MethodCall call &&
                   this.methodName.Equals(call.methodName, StringComparison.Ordinal) &&
                   this.parameterTypes.SequenceEqual(call.parameterTypes) &&
                   this.arguments.SequenceEqual(call.arguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -712421553;
                hashCode = hashCode * -1521134295 + this.methodName.GetHashCode();
                foreach (var argument in this.arguments)
                {
                    hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(argument);
                }
                foreach (var argumentType in this.parameterTypes)
                {
                    hashCode = hashCode * -1521134295 + argumentType.GetHashCode();
                }
                return hashCode;
            }
        }

        private static IEnumerable<object> ExpandParamsArgument(IList<ParameterInfo> parameters, IEnumerable<object> arguments)
        {
            if (parameters.Count <= 0) return arguments;

            var lastParameterIndex = parameters.Count - 1;

            if (!parameters[lastParameterIndex].IsDefined(typeof(ParamArrayAttribute))) return arguments;

            var expandedArguments = arguments.ToList();
            var lastArgument = expandedArguments[lastParameterIndex];
            expandedArguments.RemoveAt(lastParameterIndex);
            expandedArguments.AddRange(((IEnumerable)lastArgument).Cast<object>());
            return expandedArguments;
        }
    }
}