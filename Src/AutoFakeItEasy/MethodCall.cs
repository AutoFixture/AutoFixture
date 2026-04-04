using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoFakeItEasy;

internal class MethodCall
{
    private readonly Type _declaringType;
    private readonly string _methodName;
    private readonly IList<Type> _parameterTypes;
    private readonly IList<object> _arguments;

    public MethodCall(Type declaringType, string methodName, IEnumerable<ParameterInfo> parameters, IEnumerable<object> arguments)
    {
        _declaringType = declaringType;
        _methodName = methodName;
        var parametersList = parameters.ToList();
        _parameterTypes = parametersList.Select(p => p.ParameterType).ToList();
        _arguments = ExpandParamsArgument(parametersList, arguments).ToList();
    }

    public override bool Equals(object obj)
    {
        return obj is MethodCall call
               && _declaringType == call._declaringType
               && _methodName.Equals(call._methodName, StringComparison.Ordinal)
               && _parameterTypes.SequenceEqual(call._parameterTypes)
               && _arguments.SequenceEqual(call._arguments);
    }

    public override int GetHashCode()
    {
        var hashCode = default(HashCode);
        hashCode.Add(_declaringType);
        hashCode.Add(_methodName);
        foreach (var argument in _arguments)
        {
            hashCode.Add(argument);
        }
        foreach (var argumentType in _parameterTypes)
        {
            hashCode.Add(argumentType);
        }
        return hashCode.ToHashCode();
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