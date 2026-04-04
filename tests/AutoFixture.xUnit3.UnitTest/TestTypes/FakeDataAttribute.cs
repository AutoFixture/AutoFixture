using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class FakeDataAttribute : DataAttribute
{
    private readonly MethodInfo _expectedMethod;
    private readonly IEnumerable<object[]> _output;

    public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
    {
        _expectedMethod = expectedMethod;
        _output = output;
    }

    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
    {
        Assert.Same(_expectedMethod, testMethod);
        return _output.Select(row => new TheoryDataRow(row))
            .Cast<ITheoryDataRow>().AsReadOnlyCollection()
            .ToValueTask();
    }

    public override bool SupportsDiscoveryEnumeration() => false;
}