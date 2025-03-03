using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class FakeDataAttribute : NonTypedDataSourceGeneratorAttribute
    {
        private readonly MethodInfo expectedMethod;
        private readonly IEnumerable<object[]> output;

        public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
        {
            this.expectedMethod = expectedMethod;
            this.output = output;
        }

        public override IEnumerable<Func<object[]>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
        {
            return this.output.Select(x => new Func<object[]>(() => x));
        }
    }
}