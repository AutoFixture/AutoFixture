using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

public class DelegatingMethod : IMethod
{
    public DelegatingMethod()
    {
        OnParameters = Enumerable.Empty<ParameterInfo>;
        OnInvoke = p => null;
    }

    public IEnumerable<ParameterInfo> Parameters
    {
        get { return OnParameters(); }
    }

    public object Invoke(IEnumerable<object> parameters)
    {
        return OnInvoke(parameters);
    }

    internal Func<IEnumerable<ParameterInfo>> OnParameters { get; set; }

    internal Func<IEnumerable<object>, object> OnInvoke { get; set; }
}