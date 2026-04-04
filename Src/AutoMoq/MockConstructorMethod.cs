using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq;

internal class MockConstructorMethod : IMethod
{
    private readonly ConstructorInfo _ctor;

    internal MockConstructorMethod(ConstructorInfo ctor, ParameterInfo[] paramInfos)
    {
        _ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
        Parameters = paramInfos ?? throw new ArgumentNullException(nameof(paramInfos));
    }

    public IEnumerable<ParameterInfo> Parameters { get; }

    public object Invoke(IEnumerable<object> parameters)
    {
        var paramsArray = new object[] { parameters };
        return _ctor.Invoke(paramsArray);
    }
}