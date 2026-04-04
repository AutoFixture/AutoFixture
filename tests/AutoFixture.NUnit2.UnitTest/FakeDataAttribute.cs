using System.Collections.Generic;
using System.Reflection;
using AutoFixture.NUnit2.Addins;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest;

public class FakeDataAttribute : DataAttribute
{
    private readonly MethodInfo _expectedMethod;
    private readonly IEnumerable<object[]> _output;

    public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
    {
        _expectedMethod = expectedMethod;
        _output = output;
    }

    public override IEnumerable<object[]> GetData(MethodInfo method)
    {
        Assert.AreEqual(_expectedMethod, method);

        return _output;
    }
}