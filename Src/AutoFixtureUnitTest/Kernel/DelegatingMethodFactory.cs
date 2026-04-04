using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

public class DelegatingMethodFactory : IMethodFactory
{
    public DelegatingMethodFactory()
    {
        OnCreate = m => null;
    }

    public IMethod Create(MethodInfo methodInfo)
    {
        return OnCreate(methodInfo);
    }

    internal Func<MethodInfo, IMethod> OnCreate { get; set; }
}