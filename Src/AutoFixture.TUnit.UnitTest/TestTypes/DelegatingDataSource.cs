using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.TUnit.Internal;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class DelegatingDataSource : NonTypedDataSourceGeneratorAttribute
    {
        public IEnumerable<object[]> TestData { get; set; } = Array.Empty<object[]>();

        public override IEnumerable<Func<object[]>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
        {
            return this.TestData.Select(x => new Func<object[]>(() => x));
        }
    }
}
