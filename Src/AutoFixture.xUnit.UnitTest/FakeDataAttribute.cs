using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.Xunit.UnitTest;

public class FakeDataAttribute : DataAttribute
{
    private readonly MethodInfo _expectedMethod;
    private readonly Type[] _expectedTypes;
    private readonly IEnumerable<object[]> _output;

    public FakeDataAttribute(MethodInfo expectedMethod,
        Type[] expectedTypes, IEnumerable<object[]> output)
    {
        _expectedMethod = expectedMethod;
        _expectedTypes = expectedTypes;
        _output = output;
    }

    public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
    {
        Assert.Equal(_expectedMethod, methodUnderTest);
        Assert.True(_expectedTypes.SequenceEqual(parameterTypes));

        return _output;
    }
}