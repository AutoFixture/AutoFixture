using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class FakeDataAttribute : DataAttribute
    {
        private readonly MethodInfo expectedMethod;
        private readonly IEnumerable<object[]> output;

        public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
        {
            this.expectedMethod = expectedMethod;
            this.output = output;
        }

        public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
        {
            Assert.Same(this.expectedMethod, testMethod);
            return this.output.Select(row => new TheoryDataRow(row))
                .Cast<ITheoryDataRow>().AsReadOnlyCollection()
                .ToValueTask();
        }

        public override bool SupportsDiscoveryEnumeration() => false;
    }
}