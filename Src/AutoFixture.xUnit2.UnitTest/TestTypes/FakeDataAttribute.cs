using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

public class FakeDataAttribute : DataAttribute
{
    private readonly MethodInfo _expectedMethod;
    private readonly IEnumerable<object[]> _output;

    public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
    {
        _expectedMethod = expectedMethod;
        _output = output;
    }

    public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
    {
        Assert.Equal(_expectedMethod, methodUnderTest);

        return _output;
    }
}